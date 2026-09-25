namespace Backend.Domain.Models
{
    public class Account : Aggregate<AccountId>
    {
        private readonly List<Device> _devices = new();
        public IReadOnlyList<Device>? Devices => _devices.AsReadOnly();

        private Account() { }
        public static Account Create(AccountId id)
        {
            return new Account
            {
                Id = id,
            };
        }


        public void AddDevice(Device device)
        {
            _devices.Add(device);
        }
        public void RemoveDevice(Device device)
        {
            _devices.Remove(device);
        }

    }
}
