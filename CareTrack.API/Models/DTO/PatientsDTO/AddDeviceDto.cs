using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class AddDeviceDto
    {
        [Required]
        public int DeviceNumber { get; set; }
    }
}
