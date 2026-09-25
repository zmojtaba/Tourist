namespace Backend.Domain.ValueObjects
{
    public class DoorAddress
    {
        public int FloorNumber { get; } = default;
        public int RoomNumber { get; } = default;

        private DoorAddress(int  floorNumber, int roomNumber)
        {
            FloorNumber = floorNumber;
            RoomNumber = roomNumber;

        }

        public static DoorAddress Of(int floorNumber, int roomNumber)
        {
            if (floorNumber < 0) throw new DomainException("Floor number can not be null");
            if (roomNumber < 0) throw new DomainException("Room number can not be null");

            return new DoorAddress(floorNumber, roomNumber);
        }

    }
}
