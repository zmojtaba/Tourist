namespace Backend.Application.Features.Facilities
{
    public record DeleteFacilityCommand(Guid Id) : ICommand;
    public class DeleteFacilityHandler(IUnitOfWork uow, IFacilityRepository repo)
        : ICommandHandler<DeleteFacilityCommand>
    {
        public async Task<Unit> Handle(DeleteFacilityCommand command, CancellationToken cancellationToken)
        {
            Facility? facility = await repo.GetFacilityByIdAsync(FacilityId.Of(command.Id));
            if (facility == null)
                throw new NotFoundException("Facility with this Id not found");
            await repo.DeleteFacility(facility);
            await uow.CompleteAsync();
            return Unit.Value;
        }
    }
}
