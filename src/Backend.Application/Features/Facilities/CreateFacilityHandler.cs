using Backend.Application.Interfaces;

namespace Backend.Application.Features.Facilities
{
    public record CreateFacilityCommand(string Name, FacilityType Type) : ICommand<Facility>;


    public class CreateFacilityCommandValidator : AbstractValidator<CreateFacilityCommand>
    {
        public CreateFacilityCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Type).NotEmpty().WithMessage("Type is required")
                .IsInEnum()
                .WithMessage($"Type must be one of: {string.Join(", ", Enum.GetNames<FacilityType>())}");
        }
    }

    public class CreateFacilityHandler(IFacilityRepository facilityRepo, IUnitOfWork uow) : ICommandHandler<CreateFacilityCommand, Facility>
    {
        public async Task<Facility> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
        {
            Facility facility = Facility.Create(request.Name, request.Type);
            await facilityRepo.AddFacilityAsync(facility);
            await uow.CompleteAsync();
            return facility;
        }
    }
}
