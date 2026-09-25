namespace Backend.Domain.ValueObjects
{
    public record CameraId
    {
        public Guid Value { get; }
        private CameraId(Guid value) => Value = value;
        public static CameraId Of(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(
                    "Camera id cannot be empty.",
                    nameof(value));
            return new CameraId(value);
        }
    }
}
