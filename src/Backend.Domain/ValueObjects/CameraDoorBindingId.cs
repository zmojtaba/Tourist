namespace Backend.Domain.ValueObjects
{
    public record CameraDoorBindingId
    {
        public Guid Value { get; }
        private CameraDoorBindingId(Guid value) => Value = value;
        public static CameraDoorBindingId Of(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(
                    "Camera Door Binding id cannot be empty.",
                    nameof(value));
            return new CameraDoorBindingId(value);
        }
    }
}
