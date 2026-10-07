namespace Backend.Infrustructure.Services.Frames
{
    public static class RabbitTopology
    {
        public const string DetectionQueue = "frames.detect";
        public const string RecognitionQueue = "persons.recognize";
        public const string ResultQueue = "detections.results";

        public static async Task DeclareAllAsync(
            IChannel channel,
            int detectionMaxLength,
            int recognitionMaxLength,
            CancellationToken ct = default)
        {
            await channel.QueueDeclareAsync(
                queue: DetectionQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    ["x-max-length"] = detectionMaxLength,
                    ["x-overflow"] = "drop-head"
                },
                cancellationToken: ct);

            await channel.QueueDeclareAsync(
                queue: RecognitionQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    ["x-max-length"] = recognitionMaxLength,
                    ["x-overflow"] = "drop-head"
                },
                cancellationToken: ct);

            await channel.QueueDeclareAsync(
                queue: ResultQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: ct);
        }
    }
}
