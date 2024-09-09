using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO
{
    public class UpdateShiftDto
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
    }
}
