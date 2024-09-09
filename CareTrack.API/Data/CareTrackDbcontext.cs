using CareTrack.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CareTrack.API.Data
{
    public class CareTrackDbcontext : DbContext
    {
        public CareTrackDbcontext(DbContextOptions<CareTrackDbcontext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftAssignment> ShiftAssignments { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }
        


    }
}
