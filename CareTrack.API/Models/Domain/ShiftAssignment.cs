namespace CareTrack.API.Models.Domain
{
    public class ShiftAssignment
    {

        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ShiftId { get; set; }
        public string? Status { get; set; }

        // Navigation Properties
        public Employee Employee { get; set; }
        public Shift Shift { get; set; }
    }
}
