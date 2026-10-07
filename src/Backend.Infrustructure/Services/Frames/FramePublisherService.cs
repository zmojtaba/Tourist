using Backend.Application.Common.Redis;
using Backend.Application.Interfaces.Frames;

namespace Backend.Infrustructure.Services.Frames
{
    public sealed class FramePublisherService : BackgroundService
    {
        private readonly FrameChannel _channel;
        private readonly IRawFrameStore _rawFrameStore;
        private readonly IFrameStateStore _stateStore;
        private readonly FramePipelineOptions _opts;
        private readonly ILogger<FramePublisherService> _logger;

        public FramePublisherService(
            FrameChannel channel,
            IOptions<FramePipelineOptions> opts,
            ILogger<FramePublisherService> logger,
            IRawFrameStore rawFrameStore,
            IFrameStateStore stateStore)
        {
            _channel = channel;
            _opts = opts.Value;
            _logger = logger;
            _rawFrameStore = rawFrameStore;
            _stateStore = stateStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (RawFrame frame in _channel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        await ProcessFrameAsync(
                            frame,
                            stoppingToken);
                    }
                    catch (OperationCanceledException)
                        when (stoppingToken.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to process frame from camera {CameraId}",
                            frame.CameraId);
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // graceful shutdown
            }
            finally
            {
                _logger.LogInformation("FramePublisherService stopped");
            }
        }

        private async Task ProcessFrameAsync(
            RawFrame frame,
            CancellationToken cancellationToken)
        {
            Guid frameId = Guid.NewGuid();

            var blobKey = RedisKeys.RawFrame(frameId);

            await _rawFrameStore.SaveAsync(
                blobKey,
                frame.JpegBytes,
                TimeSpan.FromSeconds(_opts.FrameTtlSeconds),
                cancellationToken);

            var signal = new FrameSignal(
                frameId,
                frame.CameraId.Value,
                blobKey,
                frame.TimestampMs);

            await _stateStore.SaveAsync(signal, TimeSpan.FromSeconds(_opts.FrameTtlSeconds), cancellationToken);

        }
    }
}
