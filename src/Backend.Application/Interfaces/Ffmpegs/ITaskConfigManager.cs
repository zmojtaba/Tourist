namespace Backend.Application.Interfaces.Ffmpegs
{
    public interface ITaskConfigManager
    {
        Task<TaskConfig> AddOrUpdateTaskConfigAsync(CameraId sourceId, TaskConfig config);
        Task<TaskConfig> RemoveTaskConfigAsync(CameraId sourceId);
        IReadOnlyDictionary<CameraId, TaskConfig> TaskConfigs { get; }
        List<TaskConfig> GetAll();          // NEW
        TaskConfig? GetByCameraId(CameraId sourceId);
    }
}
