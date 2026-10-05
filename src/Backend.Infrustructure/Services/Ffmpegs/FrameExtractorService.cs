namespace Backend.Infrustructure.Services.Ffmpegs
{
    public class FrameExtractorService : IFrameExtractorService
    {
        private readonly IFFmpegProcessFactory _processFactory;
        private readonly ITaskConfigManager _taskConfigManager;
        private readonly IErrorHandler _errorHandler;
        private readonly IFrameBufferProcessor _frameBufferProcessor;
        private readonly IServiceScopeFactory _factory;
        private readonly ILogger<FrameExtractorService> _logger;

        public FrameExtractorService(
            IFFmpegProcessFactory processFactory,
            ITaskConfigManager taskConfigManager,
            IErrorHandler errorHandler,
            IFrameBufferProcessor frameBufferProcessor,
            ILogger<FrameExtractorService> logger,
            IServiceScopeFactory factory)
        {
            _processFactory = processFactory;
            _taskConfigManager = taskConfigManager;
            _errorHandler = errorHandler;
            _frameBufferProcessor = frameBufferProcessor;
            _logger = logger;
            _factory = factory;
        }

        public async Task ExtractFramesAsync(CameraId sourceId, string streamUrl, CancellationToken cancellationToken)
        {
            var process = _processFactory.CreateProcess(streamUrl);

            //important : i think cancellationToken should create here. now handler cancellationtoken passes to processStreamAsync
            var cts = new CancellationTokenSource();


            try
            {
                // Only this part is awaited - it's fast.
                await StartFFmpegProcessAsync(process, sourceId, cts);
            }
            catch
            {
                // Startup failed -> clean up and surface error to the handler.
                cts.Cancel();
                cts.Dispose();
                TryKillAndDispose(process);
                throw;
            }

            _ = Task.Run(() => RunExtractionInBackgroundAsync(process, sourceId, cts),
                         CancellationToken.None);
        }

        private void TryKillAndDispose(Process process)
        {
            if (process == null) return;
            try
            {
                if (!process.HasExited) process.Kill(entireProcessTree: true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error killing FFmpeg process");
            }
            finally
            {
                process.Dispose();
            }
        }


        private async Task RunExtractionInBackgroundAsync(
            Process process, CameraId sourceId, CancellationTokenSource cts)
        {
            try
            {
                await _frameBufferProcessor.ProcessStreamAsync(
                    process.StandardOutput.BaseStream,
                    sourceId,
                    cts.Token);

                await WaitForProcessExitAsync(process, cts.Token);
                ValidateProcessExit(process);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Frame extraction cancelled for camera {CameraId}", sourceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Frame extraction failed for camera {CameraId}", sourceId);
            }
            finally
            {
                // 1. Always tear down the process first. Never let cleanup throw.
                try
                {
                    await CleanupAsync(process, sourceId, cts);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Cleanup failed for camera {CameraId}", sourceId);
                }

                // 2. Always persist the "stopped" state last.
                await MarkCameraAsStoppedAsync(sourceId);
            }
        }

        private async Task MarkCameraAsStoppedAsync(CameraId sourceId)
        {
            try
            {
                using var scope = _factory.CreateScope();
                var cameraRepo = scope.ServiceProvider.GetRequiredService<ICameraRepository>();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var camera = await cameraRepo.GetCameraByIdAsync(sourceId);
                if (camera is null)
                {
                    _logger.LogWarning(
                        "Camera {CameraId} disappeared before we could mark it stopped", sourceId);
                    return;
                }

                // Idempotency: the stop API may have already disabled it.
                if (camera.Status == CameraStatus.Disabled)
                {
                    _logger.LogDebug(
                        "Camera {CameraId} already disabled — skipping write", sourceId);
                    return;
                }

                camera.Disable();
                await uow.CompleteAsync();

                _logger.LogInformation(
                    "Camera {CameraId} marked as Disabled after stream ended", sourceId);
            }
            catch (Exception ex)
            {
                // Absolutely must not rethrow: this is a fire-and-forget task.
                // Nothing is going to observe the exception, and the request that
                // started the camera is long gone.
                _logger.LogError(ex,
                    "Failed to persist Disabled status for camera {CameraId}", sourceId);
            }
        }


        private async Task StartFFmpegProcessAsync(Process process, CameraId sourceId, CancellationTokenSource linkedCts)
        {
            process.Start();

            var taskConfig = new TaskConfig
            {
                CancellationTokenSource = linkedCts,
                Process = process,
                ProcessId = process.Id
            };

            var added = await _taskConfigManager.AddOrUpdateTaskConfigAsync(sourceId, taskConfig);
            if (added == null)
            {
                throw new InvalidOperationException("Cannot add Task Config to manager");
            }

            // Setup error handling
            process.ErrorDataReceived += OnErrorDataReceived;
            process.BeginErrorReadLine();
        }

        public async Task StopFFmpegProcessAsync(CameraId sourceId)
        {
            TaskConfig? taskConfig = _taskConfigManager.GetByCameraId(sourceId);
            if (taskConfig == null)
                throw new NotFoundException("Camera with this Id is not running.");
            await CleanupAsync(taskConfig.Process, sourceId, taskConfig.CancellationTokenSource);
        }


        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_errorHandler.TryHandleError(e.Data, out var errorMessage))
            {
                // Handle the error appropriately
                _logger.LogError("FFmpeg error: {Message}", errorMessage.Message);
            }
        }

        private async Task WaitForProcessExitAsync(Process process, CancellationToken cancellationToken)
        {
            if (!process.HasExited)
            {
                await process.WaitForExitAsync(cancellationToken);
            }
        }

        private void ValidateProcessExit(Process process)
        {
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"FFmpeg exited with error code: {process.ExitCode}");
            }
        }

        private async Task CleanupAsync(Process process, CameraId sourceId, CancellationTokenSource cts)
        {
            // Cancel + dispose the CTS, but tolerate a second call.
            if (cts is not null)
            {
                try { cts.Cancel(); }
                catch (ObjectDisposedException) { /* already cleaned */ }
                catch (Exception ex) { _logger.LogWarning(ex, "CTS.Cancel failed"); }

                try { cts.Dispose(); }
                catch (ObjectDisposedException) { /* already disposed */ }
            }

            if (process is not null)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill(entireProcessTree: true);
                        await process.WaitForExitAsync();
                    }
                }
                catch (InvalidOperationException) { /* process already gone */ }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error cleaning up FFmpeg process for camera {CameraId}", sourceId);
                }
                finally
                {
                    try { process.Dispose(); } catch { /* ignore */ }
                }
            }

            // Also idempotent on the manager side (assumed).
            await _taskConfigManager.RemoveTaskConfigAsync(sourceId);
        }

    }


}
