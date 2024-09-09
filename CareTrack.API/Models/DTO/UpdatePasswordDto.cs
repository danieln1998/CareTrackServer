using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO
{
    public class UpdatePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
