using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace CareTrack.API.Models.Domain
{
    [Index(nameof(IdentificationNumber), IsUnique = true)]
    [Index(nameof(UserId), IsUnique = true)]
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IdentificationNumber { get; set; }
        public string Role { get; set; }
        public Guid UserId { get; set; }
    }
}
