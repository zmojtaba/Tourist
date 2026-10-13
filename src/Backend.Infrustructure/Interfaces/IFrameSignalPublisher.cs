namespace Backend.Infrustructure.Interfaces
{
    public interface IFrameSignalPublisher
    {
        Task PublishAsync(FrameSignal signal, CancellationToken ct);
    }
}
