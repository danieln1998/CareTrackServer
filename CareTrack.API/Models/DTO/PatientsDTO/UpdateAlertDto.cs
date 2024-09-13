using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class UpdateAlertDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "min 2 characters")]
        [MaxLength(255, ErrorMessage = "max 255 characters")]
        public string Name { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public Guid PatientId { get; set; }
    }
}
