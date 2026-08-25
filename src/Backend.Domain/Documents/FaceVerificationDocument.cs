namespace Backend.Domain.Documents
{
    public class FaceVerificationDocument : VerificationDocument
    {

        private readonly List<string> _faceImageUrls  = new List<string>();
        public IReadOnlyList<string> FaceImageUrls => _faceImageUrls.AsReadOnly();

        private FaceVerificationDocument() { }
        public FaceVerificationDocument(List<string> faceUrls) {
            _faceImageUrls = faceUrls;
        }

        public void Add(string faceImageUrl)
        {
            _faceImageUrls.Add(faceImageUrl);
        }

        public void Remove(string faceImageUrl)
        {
            _faceImageUrls.Remove(faceImageUrl);
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
