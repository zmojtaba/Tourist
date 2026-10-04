namespace Backend.Application.Features.Cameras
{
    public record CreateCameraCommand(Guid FacilityId, string Name, string Url, CameraSourceType CameraSourceType, CameraType Type)
        : ICommand<CreateCameraResponse>;

    public record CreateCameraResponse(Camera Camera, MediaInfo Info);

    public class CreateCameraCommandValidator : AbstractValidator<CreateCameraCommand>
    {
        public CreateCameraCommandValidator()
        {
            RuleFor(c => c.FacilityId).NotEmpty().WithMessage("Facility Id is required.");
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(c => c.Url).NotEmpty().WithMessage("Url is required.");

            //RuleFor(c => c.CameraSourceType)
            //    .Must(st => st != CameraSourceType.File).WithMessage("For adding a Video file as Camera please upload File first.");

            RuleFor(c => c)
                .Must(c =>
                    CameraSourceValidator.IsValid(
                        c.CameraSourceType,
                        c.Url))
                .WithMessage(c =>
                    GetInvalidSourceMessage(c.CameraSourceType));

        }

        private static string GetInvalidSourceMessage(
            CameraSourceType sourceType)
        {
            return sourceType switch
            {
                CameraSourceType.RTSP =>
                    "RTSP camera requires a valid rtsp:// or rtsps:// URL.",

                CameraSourceType.HTTP =>
                    "HTTP camera requires a valid http:// or https:// URL.",

                CameraSourceType.HLS =>
                    "HLS camera requires a valid http:// or https:// URL.",

                CameraSourceType.File =>
                    "File camera requires a valid absolute file path or file:// URL.",

                CameraSourceType.WebRTC =>
                    "WebRTC camera requires a valid http:// or https:// signaling URL.",

                CameraSourceType.USB =>
                    "USB camera requires a valid device identifier.",

                _ =>
                    "Invalid camera source type."
            };
        }
    }

    public class CreateCameraHandler(IMediaProbeService mediaProbeService, 
        IFacilityRepository facilityRepo,
        ICameraRepository cameraRepo,
        IUnitOfWork uow) 
        : ICommandHandler<CreateCameraCommand, CreateCameraResponse>
    {
        public async Task<CreateCameraResponse> Handle(CreateCameraCommand command, CancellationToken cancellationToken)
        {
            Facility? facility = await facilityRepo.GetFacilityByIdAsync(FacilityId.Of(command.FacilityId));
            if (facility == null)
                throw new NotFoundException("Facility Not Found.");

            var mediaInfo = await mediaProbeService.GetMediaInfoAsync(
                command.Url,
                cancellationToken);

            if (!mediaInfo.IsSuccess)
            {
                throw new ApplicationException(
                    $"Unable to read camera source: {mediaInfo.Error}");
            }

            Camera camera = Camera.Create(
                FacilityId.Of(command.FacilityId),
                command.Name,
                command.Url,
                command.CameraSourceType,
                command.Type
                );

            try
            {
                await cameraRepo.AddCameraAsync(camera);
                await uow.CompleteAsync();
            }catch(DbUpdateException ex)
            {
                throw new BadRequestException("Facility already has Camera with this name");
            }catch (Exception ex)
            {
                throw new InternalServerException(ex.ToString());
            }
            return new CreateCameraResponse( camera, mediaInfo);
        }
    }
}
