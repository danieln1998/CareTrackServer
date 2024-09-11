using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class UpdateRoomDto
    {
        [Required]
        public int RoomNumber { get; set; }
    }
}
