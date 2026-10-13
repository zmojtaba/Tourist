namespace Backend.Infrustructure.Services.RabbitMq
{
    public sealed class RabbitMqRecognitionSignalPublisher
        : RabbitMqPublisherBase, IRecognitionSignalPublisher
    {
        private readonly ILogger<RabbitMqRecognitionSignalPublisher> _logger;

        public RabbitMqRecognitionSignalPublisher(
            IRabbitMqConnectionProvider connectionProvider,
            IOptions<RabbitMqOptions> options,
            ILogger<RabbitMqRecognitionSignalPublisher> logger)
            : base(connectionProvider, options)
        {
            _logger = logger;
        }

        public async Task PublishAsync(RecognitionSignal signal, CancellationToken ct)
        {
            await PublishJsonAsync(
                signal,
                exchange: Options.ExchangeRecognitionName,
                routingKey: Options.RoutingKeyRecognition,
                messageId: signal.DetectionId.ToString(),
                type: nameof(RecognitionSignal),
                ct: ct);

            _logger.LogDebug(
                "Published RecognitionSignal {DetectionId} (label={Label}, conf={Confidence:F2}) for camera {CameraId}",
                signal.DetectionId, signal.Label, signal.Confidence, signal.CameraId);
        }
    }
}
