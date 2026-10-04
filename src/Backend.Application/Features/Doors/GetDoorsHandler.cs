namespace Backend.Application.Features.Doors
{
    public record GetDoorsQuery() : IQuery<List<Door>>;
    public class GetDoorsHandler(IDoorRepository repo) : IQueryHandler<GetDoorsQuery, List<Door>>
    {
        public async Task<List<Door>> Handle(GetDoorsQuery request, CancellationToken cancellationToken)
        {
            List<Door> doors = await repo.GetDoorsAsync();
            if (!doors.Any())
                throw new NotFoundException("Doors not found.");

            return doors;
        }
    }
}
