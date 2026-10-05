namespace Backend.Infrustructure.Services.Ffmpegs
{
    public class TaskConfigManager : ITaskConfigManager
    {
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private ConcurrentDictionary<CameraId, TaskConfig> _taskConfigs { get; } = new();
        public IReadOnlyDictionary<CameraId, TaskConfig> TaskConfigs => _taskConfigs;

        public async Task<TaskConfig?> AddOrUpdateTaskConfigAsync(CameraId sourceId, TaskConfig config)
        {
            await _lock.WaitAsync();
            try
            {
                if (_taskConfigs.ContainsKey(sourceId)) return null;
                _taskConfigs[sourceId] = config;
                return config;
            }
            catch
            {
                return null;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<TaskConfig?> RemoveTaskConfigAsync(CameraId sourceId)
        {
            await _lock.WaitAsync();
            try
            {
                if (_taskConfigs.TryRemove(sourceId, out var removedConfig))
                {
                    return removedConfig; // return the removed config
                }

                return null; // cameraId not found
            }
            catch
            {
                // Something went wrong, return null
                return null;
            }
            finally
            {
                _lock.Release();
            }

        }

        public List<TaskConfig> GetAll()
            => _taskConfigs.Values.ToList();   // snapshot, thread-safe (ConcurrentDictionary)

        public TaskConfig? GetByCameraId(CameraId sourceId)
            => _taskConfigs.TryGetValue(sourceId, out var cfg) ? cfg : null;


    }
}
