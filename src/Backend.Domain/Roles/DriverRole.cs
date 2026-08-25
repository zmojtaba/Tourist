using Backend.Domain.Models;

namespace Backend.Domain.Roles
{
    public class DriverRole : AgentRole
    {
        public override string RoleName => "Driver";
        public DriverRole(AccountId accountId) : base(accountId) { }


        private readonly List<Vehicle> _vehicles = new();
        public IReadOnlyList<Vehicle> Vehicles => _vehicles.AsReadOnly();
        public void AddVehicle(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
            _vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
            _vehicles.Remove(vehicle);
        }

    }
}
