namespace Backend.Application.Interfaces.Frames
{
    public interface IFramePublisher
    {
        ValueTask PublishAsync(RawFrame frame, CancellationToken ct);
    }
}
