namespace Backend.Application.Interfaces
{
    public interface IFacilityRepository
    {
        public Task<Facility> AddFacilityAsync(Facility facility);
        public Task<Facility?> GetFacilityByIdAsync(FacilityId id);
        public Task<List<Facility>> GetAllFacilitiesAsync();
        public Task DeleteFacility(Facility facility);
    }
}
