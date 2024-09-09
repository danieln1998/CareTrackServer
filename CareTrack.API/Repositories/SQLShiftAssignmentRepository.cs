using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLShiftAssignmentRepository : IShiftAssignmentRepository
    {
        private readonly CareTrackDbcontext dbContext;

        public SQLShiftAssignmentRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ShiftAssignment> CreateAsync(ShiftAssignment shiftAssignment)
        {
            await dbContext.ShiftAssignments.AddAsync(shiftAssignment);
            await dbContext.SaveChangesAsync();
            return shiftAssignment;
        }

        public async Task<ShiftAssignment?> DeleteAsync(Guid id)
        {
            var existingShiftAssignment = await dbContext.ShiftAssignments.FirstOrDefaultAsync(r => r.Id == id);
            if (existingShiftAssignment == null)
            {
                return null;
            }
            dbContext.ShiftAssignments.Remove(existingShiftAssignment);
            await dbContext.SaveChangesAsync();

            return existingShiftAssignment;

        }

        public async Task<List<ShiftAssignment>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? filterQueryII = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var shiftAssignments = dbContext.ShiftAssignments.Include("Employee").Include("Shift").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("ShiftId", StringComparison.OrdinalIgnoreCase))
                {
                    shiftAssignments = shiftAssignments.Where(r => r.ShiftId.ToString().Equals(filterQuery));

                    if (!string.IsNullOrWhiteSpace(filterQueryII))
                    {
                        shiftAssignments = shiftAssignments.Where(r => r.EmployeeId.ToString().Equals(filterQueryII));
                    }
                }
                else if (filterOn.Equals("EmployeeId", StringComparison.OrdinalIgnoreCase))
                {
                    shiftAssignments = shiftAssignments.Where(r => r.EmployeeId.ToString().Equals(filterQuery));
                }
            }

            

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Date", StringComparison.OrdinalIgnoreCase))
                {
                    shiftAssignments = isAscending
                        ? shiftAssignments.OrderBy(sa => sa.Shift.StartTime.Date)
                        : shiftAssignments.OrderByDescending(sa => sa.Shift.StartTime.Date);
                }
                
            }

            
            var skipResults = (pageNumber - 1) * pageSize;
            return await shiftAssignments.Skip(skipResults).Take(pageSize).ToListAsync();


        }

        public async Task<ShiftAssignment?> GetByIdAsync(Guid id)
        {
            return await dbContext.ShiftAssignments.Include("Employee").Include("Shift").FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ShiftAssignment?> UpdateAsync(Guid id, ShiftAssignment shiftAssignment)
        {
            var existingShiftAssignment = await dbContext.ShiftAssignments.FirstOrDefaultAsync(r => r.Id == id);


            if (existingShiftAssignment == null)
            {
                return null;
            }

            existingShiftAssignment.EmployeeId = shiftAssignment.EmployeeId;
            existingShiftAssignment.ShiftId = shiftAssignment.ShiftId;


            await dbContext.SaveChangesAsync();

            return existingShiftAssignment;
        }

    }
}
