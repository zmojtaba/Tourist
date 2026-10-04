namespace Backend.Infrustructure.Repository
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly ApplicationDbContext _context;

        public FacilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Facility> AddFacilityAsync(Facility facility)
        {
            await _context.Facilities.AddAsync(facility);
            return facility;
        }

        public Task DeleteFacility(Facility facility)
        {
            _context.Facilities.Remove(facility);
            return Task.CompletedTask;
        }

        public async Task<List<Facility>> GetAllFacilitiesAsync()
        {
            return await _context.Facilities.AsNoTracking().ToListAsync();
        }

        public Task<Facility?> GetFacilityByIdAsync(FacilityId id)
        {
            return _context.Facilities.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
