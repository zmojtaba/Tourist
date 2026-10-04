namespace Backend.Application.Interfaces
{
    public interface IMediaService
    {
        public Task<MediaUploadResult> UploadAsync(Stream bodyStream, string contentType);
        public Task DeleteMediaFilesAsync(string streamUrl, bool addBaseAddress = false);
    }
}
