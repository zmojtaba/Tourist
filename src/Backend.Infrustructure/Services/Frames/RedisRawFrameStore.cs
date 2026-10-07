using Backend.Application.Interfaces.Frames;

namespace Backend.Infrustructure.Services.Frames
{
    public sealed class RedisRawFrameStore : IRawFrameStore
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisRawFrameStore> _logger;

        public RedisRawFrameStore(
            IConnectionMultiplexer redis,
            ILogger<RedisRawFrameStore> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task SaveAsync(
            string key,
            ReadOnlyMemory<byte> jpegBytes,
            TimeSpan expiration,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();

            await database.StringSetAsync(
                key,
                //** creates a copy. The allocation/copy can become significant.
                jpegBytes.ToArray(),
                expiration);

            _logger.LogDebug(
                "Stored raw frame {BlobKey} in Redis with expiration {Expiration}",
                key,
                expiration);
        }
        public async Task<byte[]?> GetAsync(
              string key,
              CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();

            RedisValue value = await database.StringGetAsync(key);

            if (!value.HasValue)
            {
                return null;
            }

            return (byte[])value!;
        }

        public async Task<bool> DeleteAsync(
            string key,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();

            return await database.KeyDeleteAsync(key);
        }

    }
}
