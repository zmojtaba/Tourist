namespace Backend.Domain.Models
{
    public class Camera : Aggregate<CameraId>
    {
        public FacilityId FacilityId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Url { get; private set; } = string.Empty;
        public CameraSourceType CameraSourceType { get; private set; } = CameraSourceType.File;
        public CameraType Type { get; private set; }
        public CameraStatus Status { get; private set; } 


        private Camera() { }

        public static Camera Create(FacilityId facilityId, string name, string url, CameraSourceType sourcetype, CameraType type)
        {
            if (facilityId == null) throw new DomainException("FacilityId is required");
            if (string.IsNullOrEmpty(name)) throw new DomainException("Camera Name cannot be empty.");
            if (string.IsNullOrEmpty(url)) throw new DomainException("Camera Url cannot be empty.");
            ArgumentNullException.ThrowIfNull(nameof(type));

            return new()
            {
                Id = CameraId.Of(Guid.NewGuid()),
                Name = name,
                Url = url,
                CameraSourceType = sourcetype,
                Type = type,
                FacilityId = facilityId
            };

        }

        public void Disable()
        {
            Status = CameraStatus.Disabled;
        }

        public void Activate()
        {
            Status = CameraStatus.Active;
        }


    }
}
