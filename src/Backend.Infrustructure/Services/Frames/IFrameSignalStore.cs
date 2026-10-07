using Backend.Application.Common.Redis;
using Backend.Application.Interfaces.Frames;

namespace Backend.Infrustructure.Services.Frames
{
    public sealed class IFrameSignalStore : IFrameStateStore
    {
        private static readonly JsonSerializerOptions JsonOptions =
            new(JsonSerializerDefaults.Web);

        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<IFrameSignalStore> _logger;

        public IFrameSignalStore(
            IConnectionMultiplexer redis,
            ILogger<IFrameSignalStore> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task SaveAsync(
            FrameSignal signal,
            TimeSpan expiration,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();

            var key = RedisKeys.FrameSignal(signal.Id);

            var json = JsonSerializer.Serialize(
                signal,
                JsonOptions);

            await database.StringSetAsync(
                key,
                json,
                expiration);

            _logger.LogDebug(
                "Stored frame signal {FrameId} in Redis with expiration {Expiration}",
                signal.Id,
                expiration);
        }

        public async Task<FrameSignal?> GetAsync(
            Guid frameId,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();

            var key = RedisKeys.FrameSignal(frameId);

            RedisValue value = await database.StringGetAsync(key);

            if (!value.HasValue)
            {
                return null;
            }

            return JsonSerializer.Deserialize<FrameSignal>(
                value!,
                JsonOptions);
        }

        public async Task<bool> DeleteAsync(
            Guid frameId,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();

            var key = RedisKeys.FrameSignal(frameId);

            return await database.KeyDeleteAsync(key);
        }
    }
}
