namespace Backend.Application.Features.Doors
{
    public record CreateDoorCommand(Guid FacilityId, string Name, DoorType Type, int RoomNumber) : ICommand<Door>;

    public class CreateDoorValidator : AbstractValidator<CreateDoorCommand>
    {
        public CreateDoorValidator()
        {
            RuleFor(c => c.FacilityId)
                .NotEmpty().WithMessage("Facility Id is required.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(c => c.Type)
                .IsInEnum().WithMessage($"Type is not valid. valid type is: {string.Join(", ", Enum.GetNames<DoorType>())}");

            RuleFor(c => c.RoomNumber)
                .GreaterThanOrEqualTo(0).WithMessage("Room Number must be greater than or equal to zero.");
        }
    }

    public class CreateDoorHandler(IFacilityRepository facilityRepo, IDoorRepository doorRepo, IUnitOfWork uow) : ICommandHandler<CreateDoorCommand, Door>
    {
        public async Task<Door> Handle(CreateDoorCommand command, CancellationToken cancellationToken)
        {

            Facility? facility = await facilityRepo.GetFacilityByIdAsync(FacilityId.Of(command.FacilityId));
            if (facility == null)
                throw new NotFoundException("Facility with this Id not found.");

            Door door = Door.Create(
                facility.Id,
                command.Name,
                command.Type,
                command.RoomNumber
                );
            try
            {
                await doorRepo.AddDoorAsync(door);
                await uow.CompleteAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new BadRequestException("This Facility has already this room number");
            }
            catch(Exception ex)
            {
                throw new BadRequestException($"Error: \n \n {ex.ToString()} ");
            }

            return door;

        }
    }
}
