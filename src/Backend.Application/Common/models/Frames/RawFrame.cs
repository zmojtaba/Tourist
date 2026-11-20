namespace Backend.Application.Common.models.Frames
{
    public sealed record RawFrame(CameraId CameraId, ReadOnlyMemory<byte> JpegBytes, long TimestampMs);
}
