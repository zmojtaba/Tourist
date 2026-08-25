namespace Backend.Domain.Documents
{
    public class VehicleVerificationDocument : VerificationDocument
    {
        public string VehicleImageUrl { get; private set; }
        public string Model { get; private set; }
        public string Color { get; private set; }
        public int Year { get; private set; }

        private VehicleVerificationDocument() { }

        public VehicleVerificationDocument(
            string imageUrl,
            string model,
            string color,
            int year)
        {
            VehicleImageUrl = imageUrl;
            Model = model;
            Color = color;
            Year = year;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
