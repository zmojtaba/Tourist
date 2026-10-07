namespace Backend.Application.Common.models.Frames
{
    public sealed class FramePipelineOptions
    {
        public const string SectionName = "FramePipeline";

        /// <summary>In-process channel capacity between extractor and publisher.</summary>
        public int ChannelCapacity { get; set; } = 30;

        /// <summary>TTL for frame blobs in Redis (seconds).</summary>
        public int FrameTtlSeconds { get; set; } = 15;

        /// <summary>TTL for person crops in Redis (seconds).</summary>
        public int CropTtlSeconds { get; set; } = 30;

        /// <summary>Max messages on the frames.detect queue.</summary>
        public int DetectionQueueMaxLength { get; set; } = 300;

        /// <summary>Max messages on the persons.recognize queue.</summary>
        public int RecognitionQueueMaxLength { get; set; } = 100;
    }
}
