using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllAsync(int pageNumber = 1, int pageSize = 1000);
        Task<Device?> GetByIdAsync(Guid id);
        Task<Device> CreateAsync(Device device);
        Task<Device?> UpdateAsync(Guid id, Device device);
        Task<Device?> DeleteAsync(Guid id);
    }
}
