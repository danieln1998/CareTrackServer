namespace CareTrack.API.Models.Domain
{
    public class EmployeeShift
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ShiftId { get; set; }

        // Navigation Properties
        public Employee Employee { get; set; }
        public Shift Shift { get; set; }
    }
}
