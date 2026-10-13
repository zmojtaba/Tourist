namespace Backend.Infrustructure.Services.RabbitMq
{
    public abstract class RabbitMqPublisherBase : IAsyncDisposable
    {
        private readonly IRabbitMqConnectionProvider _connectionProvider;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private IChannel? _channel;

        protected RabbitMqOptions Options { get; }

        protected static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        protected RabbitMqPublisherBase(
            IRabbitMqConnectionProvider connectionProvider,
            IOptions<RabbitMqOptions> options)
        {
            _connectionProvider = connectionProvider;
            Options = options.Value;
        }

        private async Task<IChannel> GetChannelAsync(CancellationToken ct)
        {
            if (_channel is { IsOpen: true }) return _channel;

            await _lock.WaitAsync(ct);
            try
            {
                if (_channel is { IsOpen: true }) return _channel;

                // Publisher confirms ON → BasicPublishAsync waits for broker ACK.
                _channel = await _connectionProvider.CreateChannelAsync(
                    new CreateChannelOptions(
                        publisherConfirmationsEnabled: true,
                        publisherConfirmationTrackingEnabled: true),
                    ct);

                return _channel;
            }
            finally { _lock.Release(); }
        }

        protected async Task PublishJsonAsync<T>(
            T message,
            string exchange,
            string routingKey,
            string messageId,
            string type,
            CancellationToken ct)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, JsonOptions));
            var channel = await GetChannelAsync(ct);

            var props = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                ContentEncoding = "utf-8",
                MessageId = messageId,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                AppId = "Backend.FramePublisher",
                Type = type
            };

            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: props,
                body: body,
                cancellationToken: ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
            {
                try { await _channel.CloseAsync(); } catch { /* ignore */ }
                await _channel.DisposeAsync();
            }
            _lock.Dispose();
        }
    }
}
