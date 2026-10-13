namespace Backend.Infrustructure.Configurations
{
    public sealed class RedisOptions
    {
        public const string SectionName = "Redis";

        public string Configuration { get; set; } = "localhost:6379";
        public bool AbortOnConnectFail { get; set; } = false;
        public int ConnectTimeoutMs { get; set; } = 5000;
        public int SyncTimeoutMs { get; set; } = 5000;
        public int DefaultTtlSeconds { get; set; } = 180;
        public int ChannelCapacity { get; set; } = 500;

        public string BuildConnectionString()
        {
            return $"{Configuration}," +
                   $"abortConnect={AbortOnConnectFail.ToString().ToLowerInvariant()}," +
                   $"connectTimeout={ConnectTimeoutMs}," +
                   $"syncTimeout={SyncTimeoutMs}";
        }
    }
}
