namespace Backend.Application.Common.models.Frames
{
    public class FrameData
    {
        public CameraId SourceId { get; set; }
        public long? SequenceNumber { get; set; }
        public byte[] ImageBytes { get; set; }
        public long RecivedTimestamp { get; set; }
        public FrameData(byte[] imageBytes, CameraId sourceId)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("ImageBytes cannot be empty");

            //Id = Guid.NewGuid();
            ImageBytes = imageBytes;
            SourceId = sourceId;
            RecivedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
