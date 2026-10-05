namespace Backend.Application.Features.Cameras
{
    public record StopProcessCommand(Guid Id) : ICommand;
    public class StopProcessHandler(ICameraRepository cameraRepo, IFrameExtractorService _extractorService, IUnitOfWork uow) : ICommandHandler<StopProcessCommand>
    {
        public async Task<Unit> Handle(StopProcessCommand command, CancellationToken cancellationToken)
        {
            Camera? camera = await cameraRepo.GetCameraByIdAsync(CameraId.Of(command.Id));
            if (camera == null)
                throw new NotFoundException("Camera Not Found.");
            await _extractorService.StopFFmpegProcessAsync(camera.Id);
            camera.Disable();
            await uow.CompleteAsync();
            return Unit.Value;
        }
    }
}
