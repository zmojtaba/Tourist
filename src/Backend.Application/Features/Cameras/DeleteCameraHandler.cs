namespace Backend.Application.Features.Cameras
{
    public record DeleteCameraCommand(Guid Id) : ICommand;
    public class DeleteCameraHandler(ICameraRepository cameraRepo, IUnitOfWork uow, IMediaService mediaService) : ICommandHandler<DeleteCameraCommand>
    {
        public async Task<Unit> Handle(DeleteCameraCommand command, CancellationToken cancellationToken)
        {
            Camera? camera = await cameraRepo.GetCameraByIdAsync(CameraId.Of(command.Id));
            if (camera == null)
                throw new NotFoundException("Camera Not Found.");

            await cameraRepo.DeleteCamera(camera);
            if (camera.CameraSourceType == CameraSourceType.File)
                await mediaService.DeleteMediaFilesAsync(camera.Url);
            await uow.CompleteAsync();
            return Unit.Value;
        }
    }
}
