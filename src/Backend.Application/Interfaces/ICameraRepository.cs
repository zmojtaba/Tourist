namespace Backend.Application.Interfaces
{
    public interface ICameraRepository
    {
        public Task<Camera> AddCameraAsync(Camera camera);
        public Task<Camera?> GetCameraByIdAsync(CameraId id);
        public Task<List<Camera>> GetCamerasAsync();
        public Task DeleteCamera(Camera camera);
    }
}
