namespace Backend.Application.Common.models.Frames
{
    public sealed record RawFrame(CameraId CameraId, byte[] JpegBytes, long TimestampMs);
}
