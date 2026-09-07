namespace Backend.Domain.ValueObjects
{
    public record DeviceId
    {
        public Guid Value { get; }
        private DeviceId(Guid value) => Value = value;
        public static DeviceId Of(Guid value)
        {
            if (value == Guid.Empty) throw new DomainException("deviceId cannot be empty");
            return new DeviceId(value);
        }
    }
}
