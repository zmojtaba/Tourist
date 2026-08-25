using Backend.Domain.Models;

namespace Backend.Domain.Roles
{
    public class TranslatorRole : AgentRole
    {
        public override string RoleName => "Translator";

        private readonly List<string> _languages = new();
        public IReadOnlyList<string>  Languages => _languages.AsReadOnly();

        public decimal IeltsPoint { get; private set; }

        public TranslatorRole(AccountId accountId, decimal ieltsPoint) : base(accountId)
        {
            if (ieltsPoint < 0 || ieltsPoint > 9)
                throw new ArgumentOutOfRangeException();

            IeltsPoint = ieltsPoint;
        }
    }
}
