using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLEmployeeShiftRepository : IEmployeeShiftRepository
    {

        private readonly CareTrackDbcontext dbContext;

        public SQLEmployeeShiftRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<EmployeeShift> CreateAsync(EmployeeShift employeeShift)
        {
            employeeShift.StartTime = DateTime.Now;
            await dbContext.EmployeeShifts.AddAsync(employeeShift);
            await dbContext.SaveChangesAsync();
            return employeeShift;
        }

        public async Task<EmployeeShift?> DeleteAsync(Guid id)
        {
            var existingEmployeeShift = await dbContext.EmployeeShifts.FirstOrDefaultAsync(r => r.Id == id);
            if (existingEmployeeShift == null)
            {
                return null;
            }
            dbContext.EmployeeShifts.Remove(existingEmployeeShift);
            await dbContext.SaveChangesAsync();

            return existingEmployeeShift;

        }

        public async Task<List<EmployeeShift>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? filterQueryII = null)
        {
            
            var employeeShifts = dbContext.EmployeeShifts.Include("Employee").Include("Shift").AsQueryable();

            //filter 

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("ShiftId", StringComparison.OrdinalIgnoreCase))
                {
                    employeeShifts = employeeShifts.Where(r => r.ShiftId.ToString().Equals(filterQuery));

                    if (!string.IsNullOrWhiteSpace(filterQueryII))
                    {
                        employeeShifts = employeeShifts.Where(r => r.EmployeeId.ToString().Equals(filterQueryII));
                    }

                }

                

            }

            return await employeeShifts.ToListAsync();

            
        }

        public async Task<EmployeeShift?> GetByIdAsync(Guid id)
        {
            return await dbContext.EmployeeShifts.Include("Employee").Include("Shift").FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<EmployeeShift?> UpdateAsync(Guid id)
        {
            var existingEmployeeShift = await dbContext.EmployeeShifts.FirstOrDefaultAsync(r => r.Id == id);


            if (existingEmployeeShift == null)
            {
                return null;
            }

            existingEmployeeShift.EndTime = DateTime.Now;


            await dbContext.SaveChangesAsync();

            return existingEmployeeShift;
        }
    }
}
