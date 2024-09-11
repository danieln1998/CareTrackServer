using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllAsync(int pageNumber = 1, int pageSize = 1000);
        Task<Room?> GetByIdAsync(Guid id);
        Task<Room> CreateAsync(Room room);
        Task<Room?> UpdateAsync(Guid id, Room room);
        Task<Room?> DeleteAsync(Guid id);
    }
}
