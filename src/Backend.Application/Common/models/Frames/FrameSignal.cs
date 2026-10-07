namespace Backend.Application.Common.models.Frames
{
    public sealed record FrameSignal(Guid Id, Guid CameraId, string BlobKey, long TimestampMs);
}
