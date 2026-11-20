namespace Backend.Infrustructure.Services.Frames
{
    public sealed class RedisStore : IRedisStore
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisStore> _logger;
        private static readonly JsonSerializerOptions JsonOptions =
            new(JsonSerializerDefaults.Web);

        public RedisStore(
            IConnectionMultiplexer redis,
            ILogger<RedisStore> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task SaveAsync<T>(
            string key,
            //ReadOnlyMemory<byte> jpegBytes,
            T data,
            TimeSpan expiration,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();


            RedisValue value;


            if (data is byte[] bytes)
                value = bytes;
            else if (data is string str)
                value = str;
            else if (data is ReadOnlyMemory<byte> memory)
                value = memory.ToArray();
            else
                value = JsonSerializer.Serialize(data, JsonOptions);


            await database.StringSetAsync(
                key,
                value,
                expiration);

            _logger.LogDebug(
                "Stored raw frame {BlobKey} in Redis with expiration {Expiration}",
                key,
                expiration);
        }
        public async Task<T?> GetAsync<T>(
              string key,
              CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var database = _redis.GetDatabase();

            RedisValue value = await database.StringGetAsync(key);

            if (!value.HasValue)
            {
                return default;
            }

            if (typeof(T) == typeof(byte[]))
            {
                return (T)(object)(byte[])value!;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)(string)value!;
            }

            return JsonSerializer.Deserialize<T>(
                value!,
                JsonOptions);
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
