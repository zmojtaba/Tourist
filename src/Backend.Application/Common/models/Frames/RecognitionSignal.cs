namespace Backend.Application.Common.models.Frames
{
    public sealed record RecognitionSignal(
        Guid DetectionId,
        Guid FrameId,
        Guid CameraId,
        string CropBlobKey,
        BoundingBox Box,
        float Confidence,
        string Label,       
        long TimestampMs);

    public sealed record BoundingBox(float X, float Y, float Width, float Height);
}
