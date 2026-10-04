namespace Backend.Domain.Models
{
    public class Facility : Aggregate<FacilityId>
    {
        private Facility()
        {
        }

        private Facility(
            FacilityId id,
            string name,
            FacilityType type)
        {
            Id = id;
            Name = name;
            Type = type;
            Status = FacilityStatus.Active;
        }

        public string Name { get; private set; } = null!;
        public FacilityType Type { get; private set; }
        public FacilityStatus Status { get; private set; }


        public static Facility Create(
            string name,
            FacilityType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Facility name is required.");

            return new Facility(
                FacilityId.Of(Guid.NewGuid()),
                name,
                type);
        }

        public void Disable()
        {
            Status = FacilityStatus.Disabled;
        }

        public void Activate()
        {
            Status = FacilityStatus.Active;
        }
    }
}
