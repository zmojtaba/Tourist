namespace Backend.Application.Features.Doors
{
    public record AddDoorsCommand(Guid FacilityId, DoorType Type, int RoomNumberFrom, int RoomNumberTo) : ICommand<AddDoorsResponse>;
    public record AddDoorsResponse(List<Door> AddedDoors, List<object> FailedDoors);

    public class AddDoorsCommandValidator : AbstractValidator<AddDoorsCommand>
    {
        public AddDoorsCommandValidator()
        {
            RuleFor(c => c.FacilityId).NotEmpty().WithMessage("FacilityId is required.");
            RuleFor(c => c.RoomNumberFrom).GreaterThanOrEqualTo(0).WithMessage("RoomNumberFrom must be grather than or equal to Zero.");
            RuleFor(c => c.RoomNumberTo).GreaterThanOrEqualTo(0).WithMessage("RoomNumberFrom must be grather than or equal to Zero.");
            RuleFor(c => c).Must(c => c.RoomNumberFrom < c.RoomNumberTo).WithMessage("RoomNumberTo must grather than RoomNumberFrom.");
        }
    }

    public class AddDoorsHandler(IFacilityRepository facilityRepo, IDoorRepository doorRepo)
        : ICommandHandler<AddDoorsCommand, AddDoorsResponse>
    {
        public async Task<AddDoorsResponse> Handle(AddDoorsCommand command, CancellationToken cancellationToken)
        {
            FacilityId facilityId = FacilityId.Of(command.FacilityId);
            Facility? facility = await facilityRepo.GetFacilityByIdAsync(facilityId);
            if (facility == null)
                throw new NotFoundException("Facility Not Found.");

            List<Door> successAdded = new List<Door>();
            List<object> failedAdded = new List<object>();

            for (int i= command.RoomNumberFrom; i <= command.RoomNumberTo; i++)
            {
                Door door = Door.Create(facilityId, $"Room Number {i}", command.Type, i);

                try
                {
                    await doorRepo.AddAndSaveChangesDoorAsyn(door);
                    successAdded.Add(door);
                }catch (DbUpdateException ex)
                {
                    string errorMessage = "This Facility has already this room number";
                    failedAdded.Add(new 
                    {
                        door,
                        errorMessage
                    });
                }
                catch(Exception ex)
                {
                    string errorMessage = ex.ToString();
                    failedAdded.Add(new
                    {
                        door,
                        errorMessage
                    });
                }

            }

            return new AddDoorsResponse(successAdded, failedAdded);


        }
    }
}
