namespace Backend.Domain.Models
{
    public class Door : Aggregate<DoorId>
    {
        public string Name { get; private set; } = string.Empty;
        public DoorAddress Address { get; private set; }

        private readonly List<CameraId> _cameras = new();
        public IReadOnlyList<CameraId> Cameras => _cameras.AsReadOnly();

        private Door(){}

        public static Door Create (string name, DoorAddress address)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Door name cannot be empty.");
            ArgumentNullException.ThrowIfNull(nameof(name));

            return new()
            {
                Id = DoorId.Of(Guid.NewGuid()),
                Name = name,
                Address = address
            };
        }


        public void AddDoor(CameraId id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            if (!_cameras.Contains(id))
                _cameras.Add(id);
        }

        public void RemoveDoor(CameraId id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            _cameras.Remove(id);
        }
    }
}
