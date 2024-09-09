using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CareTrack.API.Models.DTO
{
    public class AddEmployeeRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "min 2 characters")]
        [MaxLength(255, ErrorMessage = "max 255 characters")]
        public string Name { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "min 2 characters")]
        [MaxLength(255, ErrorMessage = "max 255 characters")]
        public string IdentificationNumber { get; set; }
        [Required]
        [AllowedValues("Manager", "Shift Manager", "Employee", ErrorMessage = "Only Manager , Shift Manager, Employee Available")]
        public string Role { get; set; }
        [Required]
        public Guid UserId { get; set; }

    }
}
