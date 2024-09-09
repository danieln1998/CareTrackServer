using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IShiftRepository
    {

        Task<List<Shift>> GetAllAsync(string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000);
        Task<Shift?> GetByIdAsync(Guid id);
        Task<Shift> CreateAsync(Shift shift);
        Task<Shift?> UpdateAsync(Guid id, Shift shift);
        Task<Shift?> DeleteAsync(Guid id);
        Task SetJobId(Guid id, string jobId);
    }
}
