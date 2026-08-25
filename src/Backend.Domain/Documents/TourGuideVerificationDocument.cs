namespace Backend.Domain.Documents
{
    public class TourGuideVerificationDocument : VerificationDocument
    {
        public string TourLeaderLicenseImageUrl { get; private set; }
        private TourGuideVerificationDocument() { }
        public TourGuideVerificationDocument(string tourLeaderLicenseImageUrl)
        {
            ArgumentException.ThrowIfNullOrEmpty(tourLeaderLicenseImageUrl, nameof(TourLeaderLicenseImageUrl));
            TourLeaderLicenseImageUrl = tourLeaderLicenseImageUrl;

        }
        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
