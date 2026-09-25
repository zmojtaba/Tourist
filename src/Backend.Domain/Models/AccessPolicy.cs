namespace Backend.Domain.Models
{
    public class AccessPolicy : Aggregate<AccessPolicyId>
    {
        public AccountId AccountId { get; private set; }
        public CameraId CameraId { get; private set; }
        public DoorId DoorId { get; private set; }
        public bool Enabled { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public DateTime ValidTo { get; private set; }

        private AccessPolicy() { }

        public static AccessPolicy Create(
            AccessPolicyId id,
            AccountId accountId,
            DoorId doorId,
            CameraId? cameraId,
            DateTime validFrom,
            DateTime validTo)
        {
            if (validTo <= validFrom) throw new DomainException("ValidTo must be after ValidFrom.");
            ArgumentNullException.ThrowIfNull(nameof(accountId));
            ArgumentNullException.ThrowIfNull(nameof(doorId));
            ArgumentNullException.ThrowIfNull(nameof(cameraId));
            ArgumentNullException.ThrowIfNull(nameof(validFrom));
            ArgumentNullException.ThrowIfNull(nameof(validTo));

            return new AccessPolicy
            {
                Id = id,
                AccountId = accountId,
                DoorId = doorId,
                CameraId = cameraId,
                ValidFrom = validFrom,
                ValidTo = validTo,
                Enabled = true
            };
        }


        public void Enable() => Enabled = true;
        public void Disable() => Enabled = false;

        public bool IsActiveAt(DateTime now) =>
            Enabled && now >= ValidFrom && now <= ValidTo;

        /// <summary>Does this policy authorize the given (account, camera, door) at the given time?</summary>
        public bool Authorizes(AccountId accountId, CameraId cameraId, DoorId doorId, DateTime now) =>
            AccountId == accountId
            && DoorId == doorId
            && (CameraId is null || CameraId == cameraId)
            && IsActiveAt(now);

    }
}
