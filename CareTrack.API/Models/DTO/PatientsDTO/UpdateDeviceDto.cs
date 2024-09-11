using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class UpdateDeviceDto
    {
        [Required]
        public int DeviceNumber { get; set; }
    }
}
