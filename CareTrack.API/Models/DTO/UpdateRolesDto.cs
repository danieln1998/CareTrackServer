using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO
{
    public class UpdateRolesDto
    {
        [Required]
        public string[] Roles { get; set; }
    }
}
