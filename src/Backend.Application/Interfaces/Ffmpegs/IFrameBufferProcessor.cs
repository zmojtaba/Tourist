namespace Backend.Application.Interfaces.Ffmpegs
{
    public interface IFrameBufferProcessor
    {
        Task ProcessStreamAsync(Stream outputStream, CameraId sourceId, CancellationToken cancellationToken);
    }
}
