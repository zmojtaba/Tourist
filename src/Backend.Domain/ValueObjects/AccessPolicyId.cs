namespace Backend.Domain.ValueObjects
{
    public class AccessPolicyId
    {
        public Guid Value { get; }
        private AccessPolicyId(Guid value) => Value = value;
        public static AccessPolicyId Of(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(
                    "Access Policy id cannot be empty.",
                    nameof(value));
            return new AccessPolicyId(value);
        }
    }
}
