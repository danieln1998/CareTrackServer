using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Repositories
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {

        private readonly CareTrackDbcontext dbContext;

        public SQLEmployeeRepository(CareTrackDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            await dbContext.Employees.AddAsync(employee);
            await dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> DeleteAsync(Guid id)
        {
            var existingEmployee = await dbContext.Employees.FirstOrDefaultAsync(r => r.Id == id);
            if (existingEmployee == null)
            {
                return null;
            }
            dbContext.Employees.Remove(existingEmployee);
            await dbContext.SaveChangesAsync();

            return existingEmployee;

        }

        public async Task<List<Employee>> GetAllAsync(int pageNumber = 1, int pageSize = 1000)
        {
            var employees = dbContext.Employees.AsQueryable();
            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await employees.Skip(skipResults).Take(pageSize).ToListAsync();

            
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await dbContext.Employees.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Employee?> GetByUserIdAsync(Guid userId)
        {
            return await dbContext.Employees
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<Employee?> UpdateAsync(Guid id, Employee employee)
        {
            var existingEmployee = await dbContext.Employees.FirstOrDefaultAsync(r => r.Id == id);


            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.IdentificationNumber = employee.IdentificationNumber;
            existingEmployee.Role = employee.Role;

            await dbContext.SaveChangesAsync();

            return existingEmployee;
        }
    }
}
