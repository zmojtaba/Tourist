using Backend.Application.Interfaces;
using Backend.Domain.Roles;
using Backend.Infrustructure.Data;

namespace Backend.Infrustructure.Repository
{
    public class AgentRoleRepository : IAgentRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public AgentRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DriverRole> CreateDriverRoleAsync(DriverRole role)
        {
            await _context.AgentRoles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;

        }

        public async Task<DriverRole?> GetDriverRole()
        {
            return await _context.AgentRoles.OfType<DriverRole>()
                .Include(x => x.Vehicles)
                .FirstOrDefaultAsync();
                //.ToListAsync();
        }
    }
}
