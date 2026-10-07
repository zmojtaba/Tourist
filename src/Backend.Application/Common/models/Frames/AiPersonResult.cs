namespace Backend.Application.Common.models.Frames
{
    public sealed record AiPersonResult(
        float[] Bbox,
        string? Identity,
        float IdentityConfidence,
        bool IsSystemAccount);

    public sealed record AiDetectionResult(
        string CameraId,
        long TimestampMs,
        IReadOnlyList<AiPersonResult> Persons);
}
