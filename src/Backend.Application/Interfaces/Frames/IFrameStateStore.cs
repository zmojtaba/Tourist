namespace Backend.Application.Interfaces.Frames
{
    public interface IFrameStateStore
    {
        Task SaveAsync(
            FrameSignal signal,
            TimeSpan expiration,
            CancellationToken cancellationToken);

        Task<FrameSignal?> GetAsync(
            Guid frameId,
            CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
            Guid frameId,
            CancellationToken cancellationToken);
    }
}
