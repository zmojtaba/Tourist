using Backend.Application.Features.Cameras;

namespace Backend.Application.Common.Behaviors
{
    public class MediaValidationCleanupBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMediaService _mediaService;

        public MediaValidationCleanupBehavior(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (ValidationException)
            {
                if (request is CreateCameraCommand cameraCommand)
                {
                    if (cameraCommand.CameraSourceType == CameraSourceType.File)
                        if (File.Exists(cameraCommand.Url))
                            await _mediaService.DeleteMediaFilesAsync(cameraCommand.Url);

                }
                throw;
            }
        }
    }
}
