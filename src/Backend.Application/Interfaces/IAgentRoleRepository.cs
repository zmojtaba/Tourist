using Backend.Domain.Roles;

namespace Backend.Application.Interfaces
{
    public interface IAgentRoleRepository
    {
        public Task<DriverRole> CreateDriverRoleAsync(DriverRole role);
        public Task<DriverRole> GetDriverRole();
    }
}
