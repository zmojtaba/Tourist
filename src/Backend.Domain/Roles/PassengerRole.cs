using Backend.Domain.Models;

namespace Backend.Domain.Roles
{
    public class PassengerRole : AgentRole
    {
        public override string RoleName => "Passenger";
        public PassengerRole(AccountId accountId) : base(accountId) { }
        private PassengerRole(){}
    }
}
