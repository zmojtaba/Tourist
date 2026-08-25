namespace Backend.Domain.Roles
{
    public class Vehicle : Entity<VehicleId>
    {
        //public AgentRoleId RoleId { get; private set; }
        public string Model { get; private set; }
        public int ProductDate { get; private set; }
        public int EnsurenceDate { get; private set; }
        public string Color { get; private set; }
        private readonly List<string> _images = new List<string>();
        public IReadOnlyList<string> Images => _images.AsReadOnly();


        private Vehicle() { }

        private Vehicle(string model, int productDate, int ensurenceDate,  string color)
        {
            Model = model;
            ProductDate = productDate;
            EnsurenceDate = ensurenceDate;
            Color = color;
        }
        public void AddImage(string image)
        {
            _images.Add(image);
        }
        public void RemoveImage(string image)
        {
            _images.Remove(image);
        }
        public static Vehicle Of( string model, int productDate, int EnsurenceDate, string color)
        {
            return new Vehicle( model, productDate, EnsurenceDate, color);
        }
    }
}
