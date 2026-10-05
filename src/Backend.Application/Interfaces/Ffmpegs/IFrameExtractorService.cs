namespace Backend.Application.Interfaces.Ffmpegs
{
    public interface IFrameExtractorService
    {
        Task ExtractFramesAsync(CameraId sourceId, string streamUrl, CancellationToken cancellationToken);
        public Task StopFFmpegProcessAsync(CameraId sourceId);
    }
}
