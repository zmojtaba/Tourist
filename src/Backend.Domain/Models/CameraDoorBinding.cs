namespace Backend.Domain.Models
{
    public class CameraDoorBinding : Aggregate<CameraDoorBindingId>
    {
        public FacilityId FacilityId { get; private set; }
        public CameraId CameraId { get; private set; }

        public CameraDoorBindingType Type { get; private set; } = CameraDoorBindingType.OneDoor;

        public bool IsActive { get; private set; } = false;

        private CameraDoorBinding() { }

        public static CameraDoorBinding Create(CameraId cameraId, FacilityId facilityId, CameraDoorBindingType type)
        {
            ArgumentNullException.ThrowIfNull(cameraId, nameof(cameraId));
            ArgumentNullException.ThrowIfNull(facilityId, nameof(facilityId));
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return new()
            {
                Id = CameraDoorBindingId.Of(Guid.NewGuid()),
                CameraId = cameraId,
                FacilityId = facilityId,
                Type = type
            };

        }

        public void Disable()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }


    }
}
