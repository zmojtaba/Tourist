namespace Backend.Application.Features.Facilities
{
    public record GetFacilityByIdQuery(Guid Id) : IQuery<Facility>;
    public class GetFacilityByIdHandler(IFacilityRepository facilityRepo)
        : IQueryHandler<GetFacilityByIdQuery, Facility>
    {
        public async Task<Facility> Handle(GetFacilityByIdQuery request, CancellationToken cancellationToken)
        {
            Facility? facility = await facilityRepo.GetFacilityByIdAsync(FacilityId.Of(request.Id));
            if (facility == null)
                throw new NotFoundException("Facility with this Id not found.");
            return facility;
        }
    }
}
