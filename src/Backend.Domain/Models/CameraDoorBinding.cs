namespace Backend.Domain.Models
{
    public class CameraDoorBinding : Aggregate<CameraDoorBindingId>
    {
        public CameraId CameraId { get; private set; }
        public FacilityId FacilityId { get; private set; }
        public IReadOnlyList<DoorId> DoorIds => _doorIds.AsReadOnly();
        private readonly List<DoorId> _doorIds = new List<DoorId>();

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

        public void Add(DoorId id)
        {
            if (id == null) throw new DomainException("DoorId can not be  null.");
            if (Type == CameraDoorBindingType.OneDoor && _doorIds.Count == 1)
                throw new DomainException("Camera Door Binding of Type OneDoor can not Add more than one Door");
            _doorIds.Add(id);
        }

        public void Remove(DoorId id)
        {
            _doorIds.Remove(id);
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
