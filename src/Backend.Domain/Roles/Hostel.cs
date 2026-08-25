namespace Backend.Domain.Roles
{
    public class Hostel : Entity<HostelId>
    {
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public GeoLocation AddressLocation { get; private set; }

        private readonly List<string> _images = new List<string>();
        public IReadOnlyList<string> Images => _images.AsReadOnly();

        private Hostel() { }

        public Hostel(string name, Address address, GeoLocation addressLocation)
        {
            Name = name;
            Address = address;
            AddressLocation = addressLocation;
        }

        public void AddImage(string image)
        {
            _images.Add(image);
        }
        public void RemoveImage(string image)
        {
            _images.Remove(image);
        }
    }
}
