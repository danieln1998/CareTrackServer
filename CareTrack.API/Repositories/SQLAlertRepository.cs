using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLAlertRepository : IAlertRepository
    {
        private readonly CareTrackDbcontext dbContext;

        public SQLAlertRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Alert>> GetAllAsync(int pageNumber = 1, int pageSize = 1000)
        {
            var alerts = dbContext.Alerts
                 .Include(a => a.Patient)
                      .ThenInclude(p => p.Room)
                 .Include(a => a.Patient)
                      .ThenInclude(p => p.Device)
                .AsQueryable();
            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await alerts.Skip(skipResults).Take(pageSize).ToListAsync();


        }

        public async Task<Alert?> GetByIdAsync(Guid id)
        {
                    return await dbContext.Alerts
                        .Include(a => a.Patient)
                             .ThenInclude(p => p.Room)
                         .Include(a => a.Patient)
                             .ThenInclude(p => p.Device)
                    .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Alert> CreateAsync(Alert alert)
        {
            alert.Time = DateTime.Now;
            await dbContext.Alerts.AddAsync(alert);
            await dbContext.SaveChangesAsync();
            return alert;
        }

        public async Task<Alert?> UpdateAsync(Guid id, Alert alert)
        {
            var existingAlert = await dbContext.Alerts.FirstOrDefaultAsync(r => r.Id == id);

            if (existingAlert == null)
            {
                return null;
            }

            existingAlert.Name = alert.Name;
            existingAlert.Time = alert.Time;
            existingAlert.PatientId = alert.PatientId;
           

            await dbContext.SaveChangesAsync();

            return existingAlert;
        }

        public async Task<Alert?> DeleteAsync(Guid id)
        {
            var existingAlert = await dbContext.Alerts.FirstOrDefaultAsync(r => r.Id == id);
            if (existingAlert == null)
            {
                return null;
            }
            dbContext.Alerts.Remove(existingAlert);
            await dbContext.SaveChangesAsync();

            return existingAlert;

        }
    }
}
