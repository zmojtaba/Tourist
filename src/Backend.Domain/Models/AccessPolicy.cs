namespace Backend.Domain.Models
{
    public class AccessPolicy : Aggregate<AccessPolicyId>
    {
        public AccountId AccountId { get; private set; }

        public CameraDoorBindingId CameraDoorBindingId { get; private set; }

        public DateTimeOffset ValidFrom { get; private set; }

        public DateTimeOffset ValidUntil { get; private set; }

        public AccessPolicyStatus Status { get; private set; }

        private AccessPolicy() { }

        public static AccessPolicy Create(
            AccountId accountId,
            CameraDoorBindingId cameraDoorBindingId,
            DateTimeOffset validFrom,
            DateTimeOffset validUntil)
        {
            if (validUntil <= validFrom) throw new DomainException("ValidUntil must be after ValidFrom.");
            ArgumentNullException.ThrowIfNull(nameof(accountId));
            ArgumentNullException.ThrowIfNull(nameof(cameraDoorBindingId));
            ArgumentNullException.ThrowIfNull(nameof(validFrom));
            ArgumentNullException.ThrowIfNull(nameof(validUntil));

            return new AccessPolicy
            {
                Id = AccessPolicyId.Of(Guid.NewGuid()),
                AccountId = accountId,
                ValidFrom = validFrom,
                ValidUntil = validUntil,
            };
        }


        public bool IsValidAt(DateTimeOffset time)
        {
            return Status == AccessPolicyStatus.Active
                   && time >= ValidFrom
                   && time < ValidUntil;
        }

        public void Disable()
        {
            Status = AccessPolicyStatus.Disabled;
        }

        public void Activate()
        {
            Status = AccessPolicyStatus.Active;
        }



    }
}
