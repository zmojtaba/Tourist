using Backend.Application.Interfaces;
using Backend.Domain.Roles;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Features
{
    public record CreateRoleForTestingCommand() : ICommand<DriverRole>;
    public class CreateRoleForTestingHandler(IAgentRoleRepository repo) : ICommandHandler<CreateRoleForTestingCommand, DriverRole>
    {
        public async Task<DriverRole> Handle(CreateRoleForTestingCommand request, CancellationToken cancellationToken)
        {
            //DriverRole role = new DriverRole(AccountId.Of(Guid.NewGuid()));
            //role.Id = AgentRoleId.Of(Guid.NewGuid());
            //await repo.CreateDriverRoleAsync(role);
            //return role;

            var roles = await repo.GetDriverRole();
            return roles;

        }
    }
}
