namespace Backend.Application.Common.models.Frames
{
    public sealed record PersonSignal(
        string CameraId,
        string CropKey,
        long TimestampMs,
        float[] Bbox,
        float DetectionConfidence);
}
