namespace Backend.Domain.ValueObjects
{
    public record VerificationId
    {
        public Guid Value {  get; }
        private VerificationId(Guid value) => Value = value;
        public static VerificationId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty) throw new DomainException("Value can not be empty");
            return new VerificationId(value);
        }
    }
}
