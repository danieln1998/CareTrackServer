using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IEmployeeShiftRepository
    {
        Task<List<EmployeeShift>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? filterQueryII = null);
        Task<EmployeeShift?> GetByIdAsync(Guid id);
        Task<EmployeeShift> CreateAsync(EmployeeShift employeeShift);
        Task<EmployeeShift?> UpdateAsync(Guid id);
        Task<EmployeeShift?> DeleteAsync(Guid id);
    }
}
