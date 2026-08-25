namespace Backend.Domain.ValueObjects
{
    public record Device
    {
        public string Name { get; }
        public string OperatingSystem { get; }
        public string Ip { get; }
        public string Location { get; }

        protected Device() { }
        
        private Device(string name, string operatingSystem, string ip, string location)
        {
            Name = name;
            OperatingSystem = operatingSystem;
            Ip = ip;
            Location = location;
        }

        public static Device Of(string name, string os, string ip, string location)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(os);
            ArgumentException.ThrowIfNullOrWhiteSpace(ip);
            ArgumentException.ThrowIfNullOrWhiteSpace(location);

            return new Device(name, os, ip, location);
        }

    }
}
