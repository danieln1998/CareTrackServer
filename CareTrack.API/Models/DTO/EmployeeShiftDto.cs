namespace CareTrack.API.Models.DTO
{
    public class EmployeeShiftDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public EmployeeDto Employee { get; set; }
        public ShiftDto Shift { get; set; }
    }
}
