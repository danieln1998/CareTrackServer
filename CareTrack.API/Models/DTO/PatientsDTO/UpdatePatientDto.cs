using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class UpdatePatientDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "min 2 characters")]
        [MaxLength(255, ErrorMessage = "max 255 characters")]
        public string IdentificationNumber { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "min 2 characters")]
        [MaxLength(255, ErrorMessage = "max 255 characters")]
        public string Name { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? DeviceId { get; set; }
    }
}
