namespace Backend.Infrustructure.Services
{
    public class MediaService : IMediaService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public MediaService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        private static bool IsValidExtension(string fileName, string category)
        {
            string extension = Path.GetExtension(fileName);
            if (category.Equals("video", StringComparison.OrdinalIgnoreCase))
                return CameraFileExtensions.ValidExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
            return false;

        }

        private static bool IsValidExtension(string extension)
        {
            if (
                CameraFileExtensions.ValidExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase) ||
                CameraFileExtensions.ValidExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase) ||
                CameraFileExtensions.ValidExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase) ||
                CameraFileExtensions.ValidExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase)
                ) return true;
            return false;
        }

        private static void MapToDto(MediaUploadResult dto, string key, string value)
        {
            switch (key.ToLower())
            {
                case "name":
                    dto.Name = value;
                    break;

                case "type":

                    bool canParse = Enum.TryParse<CameraType>( value, true, out CameraType type);
                    if (!canParse)
                        throw new BadRequestException($"Type is not valid, the valid type is: {string.Join(", ", Enum.GetNames<CameraType>())}");

                    dto.CameraType = type;
                    break;

                case "facilityid":
                    canParse = Guid.TryParse(value, out Guid id);
                    if (!canParse)
                        throw new BadRequestException("FacilityId must be Guid.");
                    dto.FacilityId = id;
                    break;
            }
        }

        public async Task<MediaUploadResult> UploadAsync(Stream bodyStream, string contentType)
        {
            var mediaUploadResult = new MediaUploadResult();
            string baseMediaPath = _configuration["BaseStoragePath"] ?? throw new Exception("Base Storage Not Found.");

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(contentType),
                70_000);

            var reader = new MultipartReader(boundary, bodyStream);
            MultipartSection section;

            string tempPath = Path.Combine(baseMediaPath, "uploaded-file");

            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            string streamPath = null;
            string streamFileName = null;


            while ((section = await reader.ReadNextSectionAsync()) != null)
            {

                if (!ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var disposition))
                    continue;

                // FILE
                if (MultipartRequestHelper.HasFileContentDisposition(disposition))
                {
                    string name = HeaderUtilities.RemoveQuotes(disposition.Name).Value;
                    string fileName = Path.GetFileName(HeaderUtilities.RemoveQuotes(disposition.FileName).Value);

                    //var fileExtention = Path.GetExtension(safeName);

                    if (!IsValidExtension(Path.GetExtension(fileName))) throw new Exception("File is not in valid format");

                    if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                    fileName = Guid.NewGuid().ToString("N") + "_" + fileName;
                    string storagePath = Path.Combine(tempPath, fileName);

                    if (name.Equals("video", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!IsValidExtension(fileName, "video"))
                        {
                            throw new Exception($"Invalid video file extension. Supported extensions are: {string.Join(", ", CameraFileExtensions.ValidExtensions)}");
                        }

                        streamPath = storagePath;
                        streamFileName = fileName;
                    }

                    using var target = File.Create(storagePath);
                    await section.Body.CopyToAsync(target);
                }
                // FORM FIELD
                else if (MultipartRequestHelper.HasFormDataContentDisposition(disposition))
                {
                    using var reader2 = new StreamReader(section.Body);
                    var value = await reader2.ReadToEndAsync();
                    var key = HeaderUtilities.RemoveQuotes(disposition.Name).Value;
                    try
                    {
                        MapToDto(mediaUploadResult, key, value);
                    }catch(Exception ex)
                    {
                        if (!string.IsNullOrEmpty(streamPath))
                            await DeleteMediaFilesAsync(streamPath);
                        throw;

                    }
                }
            }

            if (streamPath == null)
                throw new Exception("Media file is required.");

            mediaUploadResult.TempStreamUrl = streamPath;
            mediaUploadResult.StreamFileName = streamFileName;
            return mediaUploadResult;


        }

        public Task DeleteMediaFilesAsync(string streamUrl, bool addBaseAddress = false)
        {
            if (addBaseAddress)
            {
                streamUrl = Path.Combine(_configuration["BaseStoragePath"], streamUrl);
            }
            // delete file from disk / cloud storage
            if (File.Exists(streamUrl)) File.Delete(streamUrl);

            return Task.CompletedTask;
        }

    }

}
