namespace Backend.Domain.Models
{
    public class CameraDoorBindingDoor
    {
        public CameraDoorBindingId CameraDoorBindingId { get; private set; }
        public DoorId DoorId { get; private set; }

        private CameraDoorBindingDoor()
        {
        }

        private CameraDoorBindingDoor(
            CameraDoorBindingId cameraDoorBindingId,
            DoorId doorId)
        {
            CameraDoorBindingId = cameraDoorBindingId;
            DoorId = doorId;
        }

        public static CameraDoorBindingDoor Create(
            CameraDoorBindingId cameraDoorBindingId,
            DoorId doorId)
        {
            ArgumentNullException.ThrowIfNull(cameraDoorBindingId);
            ArgumentNullException.ThrowIfNull(doorId);

            return new CameraDoorBindingDoor(
                cameraDoorBindingId,
                doorId);
        }
    }
}
