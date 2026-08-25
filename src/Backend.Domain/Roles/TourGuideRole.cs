using Backend.Domain.Models;

namespace Backend.Domain.Roles
{
    public class TourGuideRole : AgentRole
    {
        public override string RoleName => "TourGuid";

        public string Message { get; private set; }

        public TourGuideRole(AccountId accountId, string message) : base(accountId)
        {
            Message = message;
        }
        private TourGuideRole() { }
    }
}
