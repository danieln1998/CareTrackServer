namespace CareTrack.API.Models.DTO
{
    public class ShiftAssignmentDto
    {
        public Guid Id { get; set; }
        public EmployeeDto Employee { get; set; }
        public ShiftDto Shift { get; set; }
        public string Status { get; set; }
    }
}
