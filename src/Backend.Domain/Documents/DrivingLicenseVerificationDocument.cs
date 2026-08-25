public class DrivingLicenseVerificationDocument : VerificationDocument
{
    public string LicenseNumber { get; private set; } = string.Empty;
    public DateTime ExpiryDate { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;

    private DrivingLicenseVerificationDocument() { }

    public DrivingLicenseVerificationDocument(string licenseNumber, DateTime expiryDate, string imageUrl)
    {
        LicenseNumber = licenseNumber;
        ExpiryDate = expiryDate;
        ImageUrl = imageUrl;
    }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(LicenseNumber))
            throw new Exception("License required");

        if (ExpiryDate < DateTime.UtcNow)
            throw new Exception("License expired");

        if (string.IsNullOrWhiteSpace(ImageUrl))
            throw new Exception("Image required");
    }
}