namespace Backend.Infrustructure.Configurations
{
    public sealed class RabbitMqOptions
    {
        public const string SectionName = "RabbitMq";

        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VirtualHost { get; set; } = "/";

        public string ExchangeYoloName { get; set; } = "frame.yolo";
        public string RoutingKeyYolo { get; set; } = "frame.yolo";
        public string QueueNameYolo { get; set; } = "frame.processing.yolo";

        public string ExchangeRecognitionName { set; get; } = "frame.recognition";
        public string RoutingKeyRecognition { get; set; } = "frame.recognition";
        public string QueueNameRecognition { get; set; } = "frame.processing.recognition";

    }
}
