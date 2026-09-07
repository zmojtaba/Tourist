namespace Backend.Domain.Models
{
    public class Device : Entity<DeviceId>
    {
        public AccountId AccountId { get; private set; }
        public string Name { get; private set; }
        public string OperatingSystem { get; private set; }
        public string Ip { get; private set; }
        public string Location { get; private set; }

        protected Device() { }
        
        private Device(AccountId accountId, string name, string operatingSystem, string ip, string location)
        {
            Id = DeviceId.Of(Guid.NewGuid());
            AccountId = accountId;
            Name = name;
            OperatingSystem = operatingSystem;
            Ip = ip;
            Location = location;
        }

        public static Device Create(AccountId accountId, string name, string os, string ip, string location)
        {
            ArgumentNullException.ThrowIfNull(accountId, nameof(accountId));
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(os);
            ArgumentException.ThrowIfNullOrWhiteSpace(ip);
            ArgumentException.ThrowIfNullOrWhiteSpace(location);

            return new Device(accountId, name, os, ip, location);
        }

        public void ChangeIp(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) throw new DomainException("Device Ip cannot be empty");
            Ip = ip;
        }

        public void ChangeLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location)) throw new DomainException("Device Location cannot be empty");
            Location = location;
        }

    }
}
