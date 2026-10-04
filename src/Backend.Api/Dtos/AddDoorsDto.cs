namespace Backend.Api.Dtos
{
    public class AddDoorsDto
    {
        public Guid FacilityId { get; set; }
        public DoorType Type { get; set; }
        public int RoomNumberFrom { get; set; } = default;
        public int RoomNumberTo { get; set; } = default;
    }
}
