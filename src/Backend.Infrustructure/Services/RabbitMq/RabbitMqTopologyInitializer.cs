namespace Backend.Infrustructure.Services.RabbitMq
{
    public sealed class RabbitMqTopologyInitializer
        : IHostedLifecycleService
    {
        private readonly IRabbitMqTopology _topology;
        private readonly ILogger<RabbitMqTopologyInitializer> _logger;

        public RabbitMqTopologyInitializer(
            IRabbitMqTopology topology,
            ILogger<RabbitMqTopologyInitializer> logger)
        {
            _topology = topology;
            _logger = logger;
        }

        // This is awaited BEFORE any BackgroundService.ExecuteAsync runs.
        public async Task StartingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initializing RabbitMQ topology...");
            try
            {
                await _topology.EnsureTopologyAsync(cancellationToken);
                _logger.LogInformation("**** RabbitMQ topology ready.");
            }
            catch
            {
                _logger.LogError(" *** RabbitMq can not connect.");
                throw;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StartedAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StoppedAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
