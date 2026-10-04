namespace Backend.Application.Interfaces
{
    public interface IDoorRepository
    {
        public Task<Door> AddDoorAsync(Door door);
        public Task<Door> AddAndSaveChangesDoorAsyn(Door door);
        public Task<Door?> GetDoorByIdAsync(DoorId id);
        public Task<List<Door>> GetDoorsAsync();
        public Task DeleteDoor(Door door);

    }
}
