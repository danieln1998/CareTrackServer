using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IAlertRepository
    {

        Task<List<Alert>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Alert?> GetByIdAsync(Guid id);
        Task<Alert> CreateAsync(Alert alert);
        Task<Alert?> UpdateAsync(Guid id, Alert alert);
        Task<Alert?> DeleteAsync(Guid id);
    }
}
