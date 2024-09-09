using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IEmployeeRepository
    {

        Task<List<Employee>> GetAllAsync(int pageNumber = 1, int pageSize = 1000);
        Task<Employee?> GetByIdAsync(Guid id);
        Task<Employee?> GetByUserIdAsync(Guid userId);

        Task<Employee> CreateAsync(Employee employee);
        Task<Employee?> UpdateAsync(Guid id, Employee employee);
        Task<Employee?> DeleteAsync(Guid id);
    }
}
