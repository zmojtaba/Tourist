namespace Backend.Application.Common
{
    public class MediaUploadResult
    {
        public string Name { get; set; }
        public string TempStreamUrl { get; set; }
        public string StreamFileName { get; set; }
        public CameraType CameraType { get; set; }
        public Guid FacilityId { get; set; }
    }
}
