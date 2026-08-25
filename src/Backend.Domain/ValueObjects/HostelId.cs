namespace Backend.Domain.ValueObjects
{
    public record HostelId
    {
        public Guid Value { get; }

        private HostelId(Guid value) => Value = value;
        public static HostelId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty) throw new DomainException("Value can not be empty");
            return new HostelId(value);

        }
    }
}
