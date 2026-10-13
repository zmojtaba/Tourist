namespace Backend.Infrustructure.Services.RabbitMq
{
    public sealed class RabbitMqTopology : IRabbitMqTopology
    {
        private readonly IRabbitMqConnectionProvider _connectionProvider;
        private readonly RabbitMqOptions _opts;
        private readonly ILogger<RabbitMqTopology> _logger;

        public RabbitMqTopology(
            IRabbitMqConnectionProvider connectionProvider,
            IOptions<RabbitMqOptions> opts,
            ILogger<RabbitMqTopology> logger)
        {
            _connectionProvider = connectionProvider;
            _opts = opts.Value;
            _logger = logger;
        }

        public async Task EnsureTopologyAsync(CancellationToken ct)
        {
            await using var channel = await _connectionProvider.CreateChannelAsync(null, ct);

            // ---------- YOLO pipeline ----------
            await channel.ExchangeDeclareAsync(
                _opts.ExchangeYoloName, ExchangeType.Direct,
                durable: true, autoDelete: false, arguments: null, cancellationToken: ct);

            await channel.QueueDeclareAsync(
                _opts.QueueNameYolo, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] = "frame.signals.dlx",
                    ["x-dead-letter-routing-key"] = "frame.dead"
                },
                cancellationToken: ct);

            await channel.QueueBindAsync(
                _opts.QueueNameYolo, _opts.ExchangeYoloName, _opts.RoutingKeyYolo,
                arguments: null, cancellationToken: ct);

            await channel.ExchangeDeclareAsync(
                "frame.signals.dlx", ExchangeType.Direct,
                durable: true, autoDelete: false, arguments: null, cancellationToken: ct);

            await channel.QueueDeclareAsync(
                "frame.processing.dlq", durable: true, exclusive: false, autoDelete: false,
                arguments: null, cancellationToken: ct);

            await channel.QueueBindAsync(
                "frame.processing.dlq", "frame.signals.dlx", "frame.dead",
                arguments: null, cancellationToken: ct);

            // ---------- Recognition pipeline ----------
            await channel.ExchangeDeclareAsync(
                _opts.ExchangeRecognitionName, ExchangeType.Direct,
                durable: true, autoDelete: false, arguments: null, cancellationToken: ct);

            await channel.QueueDeclareAsync(
                _opts.QueueNameRecognition, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] = "frame.recognition.dlx",
                    ["x-dead-letter-routing-key"] = "frame.recognition.dead"
                },
                cancellationToken: ct);

            await channel.QueueBindAsync(
                _opts.QueueNameRecognition, _opts.ExchangeRecognitionName, _opts.RoutingKeyRecognition,
                arguments: null, cancellationToken: ct);

            await channel.ExchangeDeclareAsync(
                "frame.recognition.dlx", ExchangeType.Direct,
                durable: true, autoDelete: false, arguments: null, cancellationToken: ct);

            await channel.QueueDeclareAsync(
                "frame.recognition.dlq", durable: true, exclusive: false, autoDelete: false,
                arguments: null, cancellationToken: ct);

            await channel.QueueBindAsync(
                "frame.recognition.dlq", "frame.recognition.dlx", "frame.recognition.dead",
                arguments: null, cancellationToken: ct);

            _logger.LogInformation(
                "RabbitMQ topology ensured: yolo[{YoloExch}/{YoloQ}], recognition[{RecExch}/{RecQ}]",
                _opts.ExchangeYoloName, _opts.QueueNameYolo,
                _opts.ExchangeRecognitionName, _opts.QueueNameRecognition);
        }
    }
}
