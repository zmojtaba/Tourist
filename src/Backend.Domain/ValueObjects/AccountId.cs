namespace Backend.Domain.ValueObjects
{
    public record AccountId
    {
        public Guid Value { get; }

        private AccountId(Guid value) => Value = value;
        public static AccountId Of (Guid value)
        {
            ArgumentNullException.ThrowIfNull (value);
            if ( value == Guid.Empty  ) throw new DomainException("Value can not be empty");
            return new AccountId(value);

        }
    }
}
