namespace Backend.Domain.Documents
{
    public class IdCardVerificationDocument : VerificationDocument
    {
        public string IdNumber { get; private set; } = string.Empty;
        public DateTime? BirthDate { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public string IdCardImageUrl { get; private set; } = string.Empty;
        public bool IsVerified { get; private set; } = false;

        private IdCardVerificationDocument() { }
        public IdCardVerificationDocument(string idNumber, string idCardImageUrl, DateTime? birthDate, DateTime? expiryDate)
        {
            ArgumentException.ThrowIfNullOrEmpty(idNumber, nameof(idNumber));
            ArgumentException.ThrowIfNullOrEmpty(idCardImageUrl, nameof(idCardImageUrl));
            if (birthDate != null)
                if (DateTime.UtcNow < birthDate) throw new DomainException("Birth date can not be in future");
            if (expiryDate != null)
                if (expiryDate < birthDate)
                    throw new DomainException("Id Card expired.");
            IdNumber = idNumber;
            BirthDate = birthDate;
            ExpiryDate = expiryDate;
            IdCardImageUrl = idCardImageUrl;
        }

        public void SetIdNumber(string idNum)
        {
            ArgumentException.ThrowIfNullOrEmpty(idNum, nameof(IdNumber));
            IdNumber = idNum;
        }
        public void SetIdCardImageUrl(string idImage)
        {
            ArgumentException.ThrowIfNullOrEmpty(idImage, nameof(IdCardImageUrl));
            IdCardImageUrl = idImage;
        }


        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
