namespace Backend.Domain.Models
{
    public class Camera : Aggregate<CameraId>
    {
        public string Name { get; private set; } = string.Empty;
        public string Url { get; private set; } = string.Empty;
        public CameraType CameraType { get; private set; } = CameraType.File;

        private readonly List<DoorId> _coveredDoors = new();
        public IReadOnlyList<DoorId> coveredDoors => _coveredDoors.AsReadOnly(); 


        private Camera() { }

        public static Camera Create(string name, string url, CameraType type)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Camera Name cannot be empty.");
            if (string.IsNullOrEmpty(url)) throw new DomainException("Camera Url cannot be empty.");
            ArgumentNullException.ThrowIfNull(nameof(url));

            return new()
            {
                Id = CameraId.Of(Guid.NewGuid()),
                Name = name,
                Url = url,
                CameraType = type
            };

        }


        public void AddDoor(DoorId id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            if(! coveredDoors.Contains(id) ) 
                _coveredDoors.Add(id);
        }

        public void RemoveDoor(DoorId id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            _coveredDoors.Remove(id);
        }




    }
}
