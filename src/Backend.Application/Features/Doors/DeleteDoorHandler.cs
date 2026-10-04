namespace Backend.Application.Features.Doors
{
    public record DeleteDoorCommand(Guid DoorId) : ICommand;
    public class DeleteDoorHandler(IDoorRepository repo, IUnitOfWork uow)
        : ICommandHandler<DeleteDoorCommand>
    {
        public async Task<Unit> Handle(DeleteDoorCommand command, CancellationToken cancellationToken)
        {

            Door? door = await repo.GetDoorByIdAsync(DoorId.Of(command.DoorId));
            if (door == null)
                throw new NotFoundException("Door Not Found");
            await repo.DeleteDoor(door);
            await uow.CompleteAsync();
            return Unit.Value;
        }
    }
}
