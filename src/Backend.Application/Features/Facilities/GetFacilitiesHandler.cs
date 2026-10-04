namespace Backend.Application.Features.Facilities
{
    public record GetFacilitiesQuery() : IQuery<List<Facility>>;
    public class GetFacilitiesHandler(IUnitOfWork uow, IFacilityRepository facilityRepo) :
        IQueryHandler<GetFacilitiesQuery, List<Facility>>
    {
        public async Task<List<Facility>> Handle(GetFacilitiesQuery request, CancellationToken cancellationToken)
        {
            List<Facility> facilities = await facilityRepo.GetAllFacilitiesAsync();
            if (!facilities.Any())
                throw new NotFoundException("Facilities not found.");
            return facilities;
        }
    }
}
