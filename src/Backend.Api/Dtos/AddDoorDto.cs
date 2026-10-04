namespace Backend.Api.Dtos
{
    public class AddDoorDto
    {
        public Guid FacilityId { get; set; }
        public string Name { get; set; } = "";
        public DoorType Type { get; set; }
        public int RoomNumber { get; set; } = default;
    }
}
