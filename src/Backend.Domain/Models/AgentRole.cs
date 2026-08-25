namespace Backend.Domain.Models
{
    public abstract class AgentRole : Aggregate<AgentRoleId>
    {
        public AccountId AccountId { get; protected set; }
        public GeoLocation? CurrentLocation { get; private set; }
        public abstract string RoleName { get; }

        protected AgentRole() { }
        public AgentRole(AccountId accountId)
        {
            AccountId = accountId;
        }

        public void UpdateLocation(double lat, double lng)
        {
            CurrentLocation = new GeoLocation(lat, lng);
        }

        //public void AddRole(AgentRole role)
        //{
        //    if (_roles.Any(r => r.RoleName == role.RoleName))
        //        throw new InvalidOperationException("Role already exists");

        //    _roles.Add(role);
        //}

        //public T? GetRole<T>() where T : AgentRole
        //{
        //    return _roles.OfType<T>().FirstOrDefault();
        //}

        //public T RequireRole<T>() where T : AgentRole
        //{
        //    var role = GetRole<T>();
        //    if (role == null)
        //        throw new Exception("Role not found");

        //    return role;
        //}

    }
}
