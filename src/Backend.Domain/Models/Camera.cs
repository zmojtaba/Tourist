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
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Camera name cannot be empty.");

            if (string.IsNullOrWhiteSpace(url))
                throw new DomainException("Camera URL cannot be empty.");

            if (!Enum.IsDefined(sourcetype))
                throw new DomainException("Invalid camera source type.");

            if (!Enum.IsDefined(type))
                throw new DomainException("Invalid camera type.");

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
