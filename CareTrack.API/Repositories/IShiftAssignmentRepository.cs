using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IShiftAssignmentRepository
    {
        Task<List<ShiftAssignment>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? filterQueryII = null, string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000);
        Task<ShiftAssignment?> GetByIdAsync(Guid id);
        Task<ShiftAssignment> CreateAsync(ShiftAssignment shiftAssignment);
        Task<ShiftAssignment?> UpdateAsync(Guid id, ShiftAssignment shiftAssignment);
        Task<ShiftAssignment?> DeleteAsync(Guid id);
    }
}
