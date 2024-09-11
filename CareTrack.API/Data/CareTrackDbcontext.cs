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
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Room)
                .WithMany()
                .HasForeignKey(p => p.RoomId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Device)
                .WithMany()
                .HasForeignKey(p => p.DeviceId)
                .OnDelete(DeleteBehavior.NoAction); 
        }




    }
}
