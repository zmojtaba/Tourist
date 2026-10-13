namespace Backend.Infrustructure.Services.RabbitMq
{
    public sealed class RabbitMqConnectionProvider : IRabbitMqConnectionProvider, IAsyncDisposable
    {
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqConnectionProvider> _logger;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private IConnection? _connection;

        public RabbitMqConnectionProvider(
            IOptions<RabbitMqOptions> options,
            ILogger<RabbitMqConnectionProvider> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IConnection> GetConnectionAsync(CancellationToken ct)
        {
            if (_connection is { IsOpen: true }) return _connection;

            await _lock.WaitAsync(ct);
            try
            {
                if (_connection is { IsOpen: true }) return _connection;

                var factory = new ConnectionFactory
                {
                    HostName = _options.HostName,
                    Port = _options.Port,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    VirtualHost = _options.VirtualHost,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
                };

                _connection = await factory.CreateConnectionAsync(ct);
                _logger.LogInformation("Connected to RabbitMQ at {Host}:{Port}",
                    _options.HostName, _options.Port);
                return _connection;
            }
            finally { _lock.Release(); }
        }

        public async Task<IChannel> CreateChannelAsync(CreateChannelOptions? options, CancellationToken ct)
        {
            var connection = await GetConnectionAsync(ct);
            return await connection.CreateChannelAsync(options, ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
            _lock.Dispose();
        }
    }

}
