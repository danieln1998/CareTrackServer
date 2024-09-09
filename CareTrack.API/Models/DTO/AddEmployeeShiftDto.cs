using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO
{
    public class AddEmployeeShiftDto
    {
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public Guid ShiftId { get; set; }
    }
}
