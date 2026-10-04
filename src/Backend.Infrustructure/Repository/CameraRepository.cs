namespace Backend.Infrustructure.Repository
{
    public class CameraRepository : ICameraRepository
    {
        private readonly ApplicationDbContext _context;

        public CameraRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Camera> AddCameraAsync(Camera camera)
        {
            await _context.Cameras.AddAsync(camera);
            return camera;
        }

        public Task DeleteCamera(Camera camera)
        {
            _context.Cameras.Remove(camera);
            return Task.CompletedTask;
        }

        public async Task<Camera?> GetCameraByIdAsync(CameraId id)
        {
            return await _context.Cameras.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Camera>> GetCamerasAsync()
        {
            return await _context.Cameras.AsNoTracking().ToListAsync();
        }
    }
}
