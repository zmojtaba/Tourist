namespace Backend.Application.Interfaces.Frames
{
    public interface IRawFrameStore
    {
        Task SaveAsync(
            string key,
            ReadOnlyMemory<byte> jpegBytes,
            TimeSpan expiration,
            CancellationToken cancellationToken);

        Task<byte[]?> GetAsync(
            string key,
            CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
            string key,
            CancellationToken cancellationToken);
    }
}
