using Backend.Application.Common;

namespace Backend.Application.Interfaces.Ffmpegs
{
    public interface IMediaProbeService
    {
        Task<MediaInfo> GetMediaInfoAsync(
            string input,
            CancellationToken cancellationToken = default);
    }
}
