namespace Backend.Domain.Models
{
    public class Verification : Aggregate<VerificationId>
    {
        private Verification() { }
        public VerificationType Type { get; private set; }
        public AccountId AccountId { get; private set; }
        public VerificationStatus VerificationStatus { get; private set; }
        public string StatusMessage { get; private set; }

        public VerificationDocument Document {  get; private set; }

        public Verification(AccountId accountId, VerificationDocument document)
        {
            AccountId = accountId;
            Document = document;
            Type = ResolveType(document);
            VerificationStatus = VerificationStatus.Pending;
            StatusMessage = "In Process";
        }

        public void SetStatus(VerificationStatus status, string message)
        {
            ArgumentNullException.ThrowIfNull(status, nameof(message));
            ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));

            VerificationStatus = status;
            StatusMessage = message;
        }
        public void Approve(string message = "")
        {
            Document.Validate(); // 🔥 important
            VerificationStatus = VerificationStatus.Approved;
            StatusMessage = message;
        }

        public void Reject(string message)
        {
            VerificationStatus = VerificationStatus.Rejected;
            StatusMessage = message;
        }

        private VerificationType ResolveType(VerificationDocument document)
        {
            return document switch
            {
                IdCardVerificationDocument => VerificationType.IdCard,
                FaceVerificationDocument => VerificationType.FaceImage,
                DrivingLicenseVerificationDocument => VerificationType.DrivingLicense,
                CarImageVerificationDocument => VerificationType.CarImage,
                CarInsuranceVerificationDocument => VerificationType.CarEnsurence,
                EnglishProficiencyVerificationDocument => VerificationType.EnglishProficiency,
                TourGuideVerificationDocument => VerificationType.TourGuide,
                AddressVerificationDocument => VerificationType.Address,
                _ => throw new ArgumentOutOfRangeException(nameof(document))
            };
        }
    }
}
