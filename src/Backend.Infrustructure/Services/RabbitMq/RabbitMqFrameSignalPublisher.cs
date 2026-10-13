namespace Backend.Infrustructure.Services.RabbitMq
{
    public sealed class RabbitMqFrameSignalPublisher : RabbitMqPublisherBase, IFrameSignalPublisher
    {
        private readonly ILogger<RabbitMqFrameSignalPublisher> _logger;

        public RabbitMqFrameSignalPublisher(
            IRabbitMqConnectionProvider connectionProvider,
            IOptions<RabbitMqOptions> options,
            ILogger<RabbitMqFrameSignalPublisher> logger)
            : base(connectionProvider, options)
        {
            _logger = logger;
        }

        public async Task PublishAsync(FrameSignal signal, CancellationToken ct)
        {
            await PublishJsonAsync(
                signal,
                exchange: Options.ExchangeYoloName,
                routingKey: Options.RoutingKeyYolo,
                messageId: signal.Id.ToString(),
                type: nameof(FrameSignal),
                ct: ct);

            _logger.LogDebug(
                "Published FrameSignal {FrameId} for camera {CameraId}",
                signal.Id, signal.CameraId);
        }
    }
}
