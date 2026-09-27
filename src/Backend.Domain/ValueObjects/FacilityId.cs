namespace Backend.Domain.ValueObjects
{
    public record FacilityId
    {
        public Guid Value { get; }
        private FacilityId(Guid value) => Value = value;
        public static FacilityId Of(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(
                    "Door id cannot be empty.",
                    nameof(value));
            return new FacilityId(value);
        }
    }
}
