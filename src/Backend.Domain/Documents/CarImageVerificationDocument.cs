namespace Backend.Domain.Documents
{
    public class CarImageVerificationDocument : VerificationDocument
    {
        public string CarIdImageUrl { get; private set; } = string.Empty;
        public string PlateNumber { get; private set; } = string.Empty;
        public string Model { get; private set; } = string.Empty;
        public string Color { get; private set; } = string.Empty;
        public int Year { get; private set; } = default;
        public bool IsVerified { get; private set; } = false;
        public readonly List<string> _carImageUrls = new List<string>();
        public IReadOnlyList<string> CarImageUrls => _carImageUrls.AsReadOnly();

        private CarImageVerificationDocument() { }

        public CarImageVerificationDocument(string carIdImageUrl, string plateNumber, string model, string color, int year)
        {
            CarIdImageUrl = carIdImageUrl;
            PlateNumber = plateNumber;
            Model = model;
            Color = color;
            Year = year;
        }

        public void AddCarImageUrl(string carImageUrl)
        {
            ArgumentException.ThrowIfNullOrEmpty(carImageUrl, nameof(carImageUrl));
            _carImageUrls.Add(carImageUrl);

        }

        public void RemoveCarImageUrl(string carImageUrl)
        {
            ArgumentException.ThrowIfNullOrEmpty(carImageUrl, nameof(carImageUrl));
            _carImageUrls.Remove(carImageUrl);
        }



        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(CarIdImageUrl))
                throw new Exception("Car image required");
        }
    }
}
