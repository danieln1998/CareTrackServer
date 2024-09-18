using CareTrack.API.Models.Domain;

namespace CareTrack.API.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync(string? filterOn = null, string? filterQuery = null, int pageNumber = 1, int pageSize = 1000);
        Task<Patient?> GetByIdAsync(Guid id);
        Task<Patient> CreateAsync(Patient patient);
        Task<Patient?> UpdateAsync(Guid id, Patient patient);
        Task<Patient?> DeleteAsync(Guid id);
    }
}
