namespace Backend.Domain.ValueObjects
{
    public record AgentRoleId
    {
        public Guid Value { get; }

        private AgentRoleId(Guid value) => Value = value;
        public static AgentRoleId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty) throw new DomainException("Value can not be empty");
            return new AgentRoleId(value);

        }
    }
}
