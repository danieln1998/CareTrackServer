using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Email]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
