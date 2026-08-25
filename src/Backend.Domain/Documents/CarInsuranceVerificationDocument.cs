namespace Backend.Domain.Documents
{
    public class CarInsuranceVerificationDocument : VerificationDocument
    {
        public string PolicyNumber { get; private set; } = string.Empty;
        public DateTime ExpiryDate { get; private set; }
        public string? DocumentUrl { get; private set; } = string.Empty;

        private CarInsuranceVerificationDocument() { }

        public CarInsuranceVerificationDocument(string policyNumber, DateTime expiryDate, string documentUrl)
        {
            PolicyNumber = policyNumber;
            ExpiryDate = expiryDate;
            DocumentUrl = documentUrl;
        }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(PolicyNumber))
                throw new Exception("Policy required");

            if (ExpiryDate < DateTime.UtcNow)
                throw new Exception("Insurance expired");
        }
    }
}
