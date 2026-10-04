namespace Backend.Api.Dtos
{
    public class AddCameraDto
    {
        public Guid FacilityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public CameraSourceType CameraSourceType { get; set; } = CameraSourceType.File;
        public CameraType Type { get; set; }
    }
}
