using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.Application.Features.Cameras
{
    public record RunningCameraDto(
        Guid CameraId,
        int ProcessId,
        bool HasExited,
        DateTimeOffset? StartTime,
        TimeSpan? TotalProcessorTime);

    public record GetRunningCamerasResult(IReadOnlyList<RunningCameraDto> Cameras);

    public record GetRunningCamerasQuery : IQuery<GetRunningCamerasResult>;


    public class GetRunningCamerasHandler: IQueryHandler<GetRunningCamerasQuery, GetRunningCamerasResult>
    {

        private readonly ITaskConfigManager _taskConfigManager;

        public GetRunningCamerasHandler(ITaskConfigManager taskConfigManager)
        {
            _taskConfigManager = taskConfigManager;
        }
        public async Task<GetRunningCamerasResult> Handle(GetRunningCamerasQuery request, CancellationToken cancellationToken)
        {
            var items = _taskConfigManager.TaskConfigs
                .Select(kv =>
                {
                    var cameraId = kv.Key;      // <-- the CameraId
                    var cfg = kv.Value;

                    bool exited = true;
                    DateTimeOffset? start = null;
                    TimeSpan? cpu = null;

                    try
                    {
                        exited = cfg.Process.HasExited;
                        if (!exited)
                        {
                            start = cfg.Process.StartTime;
                            cpu = cfg.Process.TotalProcessorTime;
                        }
                    }
                    catch { }

                    return new RunningCameraDto(
                        cameraId.Value,     // or cameraId if it's a Guid alias
                        cfg.ProcessId,
                        exited,
                        start,
                        cpu);
                })
                .ToList();

            return new GetRunningCamerasResult(items);
        }
    }
}
