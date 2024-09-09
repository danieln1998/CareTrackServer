using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLShiftRepository : IShiftRepository
    {
        private readonly CareTrackDbcontext dbContext;

        public SQLShiftRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Shift> CreateAsync(Shift shift)
        {
            await dbContext.Shifts.AddAsync(shift);
            await dbContext.SaveChangesAsync();
            return shift;
        }

        public async Task<Shift?> DeleteAsync(Guid id)
        {
            var existingShift = await dbContext.Shifts.FirstOrDefaultAsync(r => r.Id == id);
            if (existingShift == null)
            {
                return null;
            }
            dbContext.Shifts.Remove(existingShift);
            await dbContext.SaveChangesAsync();

            return existingShift;

        }

        public async Task<List<Shift>> GetAllAsync(string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var shifts = dbContext.Shifts.AsQueryable();

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Date", StringComparison.OrdinalIgnoreCase))
                {
                    shifts = isAscending
                        ? shifts.OrderBy(s => s.StartTime.Date)
                        : shifts.OrderByDescending(s => s.StartTime.Date);
                }
                else if (sortBy.Equals("StartTime", StringComparison.OrdinalIgnoreCase))
                {
                    shifts = isAscending
                        ? shifts.OrderBy(s => s.StartTime.TimeOfDay)
                        : shifts.OrderByDescending(s => s.StartTime.TimeOfDay);
                }
                else if (sortBy.Equals("EndTime", StringComparison.OrdinalIgnoreCase))
                {
                    shifts = isAscending
                        ? shifts.OrderBy(s => s.EndTime.TimeOfDay)
                        : shifts.OrderByDescending(s => s.EndTime.TimeOfDay);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await shifts.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Shift?> GetByIdAsync(Guid id)
        {
            return await dbContext.Shifts.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Shift?> UpdateAsync(Guid id, Shift shift)
        {
            var existingShift = await dbContext.Shifts.FirstOrDefaultAsync(r => r.Id == id);


            if (existingShift == null)
            {
                return null;
            }

            existingShift.StartTime = shift.StartTime;
            existingShift.EndTime = shift.EndTime;
            

            await dbContext.SaveChangesAsync();

            return existingShift;
        }

        public async Task SetJobId(Guid id ,string jobId)
        {
            var existingShift = await dbContext.Shifts.FirstOrDefaultAsync(r => r.Id == id);
            
            existingShift.JobId = jobId;

            await dbContext.SaveChangesAsync();
        }
    }
}
