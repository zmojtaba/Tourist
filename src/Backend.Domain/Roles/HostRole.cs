using Backend.Domain.Models;
using Microsoft.Extensions.Hosting;

namespace Backend.Domain.Roles
{
    public class HostRole : AgentRole
    {
        public override string RoleName => "Host";

        private readonly List<Hostel> _hostels = new();
        public IReadOnlyList<Hostel> Hostels => _hostels.AsReadOnly();

        private HostRole() { }

        public HostRole(AccountId accountId) : base(accountId)
        {
        }

        public void AddHostel(Hostel hostel)
        {
            _hostels.Add(hostel);
        }
    }
}
