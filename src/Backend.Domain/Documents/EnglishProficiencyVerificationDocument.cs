namespace Backend.Domain.Documents
{
    public class EnglishProficiencyVerificationDocument : VerificationDocument
    {
        public decimal Score { get; private set; }
        public string CertificateUrl { get; private set; } = string.Empty;

        private EnglishProficiencyVerificationDocument() { }

        public EnglishProficiencyVerificationDocument(decimal score, string url)
        {
            if (score < 0 || score > 120)
                throw new ArgumentOutOfRangeException(nameof(score));

            Score = score;
            CertificateUrl = url;
        }

        public override void Validate()
        {
            if (Score < 5)
                throw new Exception("Score too low");

            if (string.IsNullOrWhiteSpace(CertificateUrl))
                throw new Exception("Certificate required");
        }
    }
}
