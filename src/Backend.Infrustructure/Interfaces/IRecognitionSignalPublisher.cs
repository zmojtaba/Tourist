namespace Backend.Infrustructure.Interfaces
{
    public interface IRecognitionSignalPublisher
    {
        Task PublishAsync(RecognitionSignal signal, CancellationToken ct);
    }
}
