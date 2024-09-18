using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLPatientRepository : IPatientRepository
    {
        private readonly CareTrackDbcontext dbContext;

        public SQLPatientRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Patient>> GetAllAsync(string? filterOn = null, string? filterQuery = null,int pageNumber = 1, int pageSize = 1000)
        {
            var patients = dbContext.Patients.Include("Room").Include("Device").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                 if (filterOn.Equals("RoomId", StringComparison.OrdinalIgnoreCase))
                {
                    patients = patients.Where(p => p.Room.Id.ToString().Equals(filterQuery));
                }
            }

             // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await patients.Skip(skipResults).Take(pageSize).ToListAsync();


        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            return await dbContext.Patients.Include("Room").Include("Device").FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            await dbContext.Patients.AddAsync(patient);
            await dbContext.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient?> UpdateAsync(Guid id, Patient patient)
        {
            var existingPatient = await dbContext.Patients.FirstOrDefaultAsync(r => r.Id == id);

            if (existingPatient == null)
            {
                return null;
            }

            existingPatient.IdentificationNumber = patient.IdentificationNumber;
            existingPatient.Name = patient.Name;
            existingPatient.RoomId = patient.RoomId;
            existingPatient.DeviceId = patient.DeviceId;

            await dbContext.SaveChangesAsync();

            return existingPatient;
        }

        public async Task<Patient?> DeleteAsync(Guid id)
        {
            var existingPatient = await dbContext.Patients.FirstOrDefaultAsync(r => r.Id == id);
            if (existingPatient == null)
            {
                return null;
            }
            dbContext.Patients.Remove(existingPatient);
            await dbContext.SaveChangesAsync();

            return existingPatient;

        }
    }
}
