namespace Backend.Application.Common
{
    public class MediaInfo
    {
        public int Width { get; init; }

        public int Height { get; init; }

        public double? Fps { get; init; }

        public string? VideoCodec { get; init; }

        public string? Format { get; init; }

        public double? DurationSeconds { get; init; }

        public string? Error { get; init; }

        public bool IsSuccess =>
            string.IsNullOrWhiteSpace(Error) &&
            Width > 0 &&
            Height > 0;
    }
}
