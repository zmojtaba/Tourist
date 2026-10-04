namespace Backend.Infrustructure.Repository
{
    public class DoorRepository : IDoorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Door> AddDoorAsync(Door door)
        {
            await _context.Doors.AddAsync(door);
            return door;

        }

        public async Task<Door> AddAndSaveChangesDoorAsyn(Door door)
        {
            await _context.Doors.AddAsync(door);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                _context.Entry(door).State = EntityState.Detached;
                throw;
            }

            return door;
        }


        public Task DeleteDoor(Door door)
        {
            _context.Doors.Remove(door);
            return Task.CompletedTask;
        }

        public async Task<Door?> GetDoorByIdAsync(DoorId id)
        {
            return await _context.Doors.FirstOrDefaultAsync(d => d.Id == id);

        }

        public async Task<List<Door>> GetDoorsAsync()
        {
            return await _context.Doors.AsNoTracking().OrderBy(d => d.RoomNumber).ToListAsync();
        }
    }
}
