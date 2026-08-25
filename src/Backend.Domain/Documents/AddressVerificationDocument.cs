namespace Backend.Domain.Documents
{
    public class AddressVerificationDocument : VerificationDocument
    {
        public GeoLocation GeoLocation { get; private set; }
        public string LocationImage { get; private set; }

        public AddressVerificationDocument(GeoLocation geoLocation, string locationImage)
        {
            GeoLocation = geoLocation;
            LocationImage = locationImage;
        }
        private AddressVerificationDocument() { }
        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
