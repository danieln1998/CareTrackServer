using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class AddRoomDto
    {
        [Required]
        public int RoomNumber { get; set; }
    }
}
