using Microsoft.AspNetCore.Identity;

namespace CareTrack.API.Models.DTO
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public string Role { get; set; }
        public Guid UserId { get; set; }

    }
}
