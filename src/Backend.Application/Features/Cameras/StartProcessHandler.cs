namespace Backend.Application.Features.Cameras
{
    public record StartProcessCommand(Guid CameraId) : ICommand<StartProcessResult>;
    public record StartProcessResult(int StatusCode, string Message);
    public class StartProcessHandler : ICommandHandler<StartProcessCommand, StartProcessResult>
    {
        private readonly IFrameExtractorService _frameExtractorService;
        private readonly ICameraRepository _cameraRepo;
        private readonly ITaskConfigManager _taskManager;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<StartProcessHandler> _logger;

        public StartProcessHandler(IFrameExtractorService frameExtractorService, 
            ICameraRepository cameraRepo, 
            ILogger<StartProcessHandler> logger, 
            ITaskConfigManager taskManager, 
            IUnitOfWork uow)
        {
            _frameExtractorService = frameExtractorService;
            _cameraRepo = cameraRepo;
            _logger = logger;
            _taskManager = taskManager;
            _uow = uow;
        }

        public async Task<StartProcessResult> Handle(StartProcessCommand command, CancellationToken cancellationToken)
        {
            Camera? camera = await _cameraRepo.GetCameraByIdAsync(CameraId.Of(command.CameraId));
            if (camera == null)
                throw new NotFoundException("Camera Not Found.");

            TaskConfig? taskConfig = _taskManager.GetByCameraId(camera.Id);
            if (taskConfig != null)
                throw new BadRequestException("Camera is already running.");


            try
            {
                // Returns quickly: only waits until FFmpeg is running.
                await _frameExtractorService.ExtractFramesAsync(
                    camera.Id, camera.Url, cancellationToken);

                camera.Activate();
                await _uow.CompleteAsync();

                return new StartProcessResult(200, "Camera process started successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start camera process for {CameraId}", camera.Id);
                return new StartProcessResult(500, $"Failed to start camera process: {ex.Message}");
            }


        }
    }
}
