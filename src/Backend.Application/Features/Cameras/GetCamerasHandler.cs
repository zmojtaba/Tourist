namespace Backend.Application.Features.Cameras
{
    public record GetCamerasQuery() : IQuery<List<Camera>>;
    public class GetCamerasHandler(ICameraRepository cameraRepo) : IQueryHandler<GetCamerasQuery, List<Camera>>
    {
        public async Task<List<Camera>> Handle(GetCamerasQuery request, CancellationToken cancellationToken)
        {
            List<Camera> cameras = await cameraRepo.GetCamerasAsync();
            if (!cameras.Any())
                throw new NotFoundException("Camera Not Found.");

            return cameras;

        }
    }
}
