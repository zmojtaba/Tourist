namespace Backend.Application.Interfaces.Frames
{
    public interface IRedisStore
    {
        Task SaveAsync<T>(
            string key,
            //ReadOnlyMemory<byte> jpegBytes,
            T data,
            TimeSpan expiration,
            CancellationToken cancellationToken);

        Task<T?> GetAsync<T>(
            string key,
            CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
            string key,
            CancellationToken cancellationToken);
    }
}
