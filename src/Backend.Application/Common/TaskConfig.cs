namespace Backend.Application.Common
{
    public class TaskConfig
    {
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Process Process { get; set; }
        public int ProcessId { get; set; }
    }
}
