using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO
{
    public class UpdateUserNameDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Email]
        public string UserName { get; set; }

       
    }
}
