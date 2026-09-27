namespace Backend.Domain.Models
{
    public class Door : Aggregate<DoorId>
    {
        public FacilityId FacilityId { get; private set; }
        public string Name { get; private set; } = null!;
        public DoorType Type { get; private set; }
        public DoorStatus Status { get; private set; }
        public int RoomNumber { get; private set; } = default; 

        private Door()
        {
        }

        private Door(
            DoorId id,
            FacilityId facilityId,
            string name,
            DoorType type,
            int roomNumber)
        {
            Id = id;
            FacilityId = facilityId;
            Name = name;
            Type = type;
            Status = DoorStatus.Active;
            RoomNumber = roomNumber;
        }


        public static Door Create(
            FacilityId facilityId,
            string name,
            DoorType type,
            int roomNumber)
        {
            if (facilityId == null)
                throw new DomainException("Facility is required.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Door name is required.");

            if (roomNumber < 0)
                throw new DomainException("Room number cannot be negetaive");
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return new Door(
                DoorId.Of(Guid.NewGuid()),
                facilityId,
                name,
                type,
                roomNumber);
        }

        public void Disable()
        {
            Status = DoorStatus.Disabled;
        }

        public void Activate()
        {
            Status = DoorStatus.Active;
        }
    }
}
