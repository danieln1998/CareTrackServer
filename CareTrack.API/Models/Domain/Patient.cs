using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Models.Domain
{
    [Index(nameof(IdentificationNumber), IsUnique = true)]
    [Index(nameof(DeviceId), IsUnique = true)]


    public class Patient
    {
        public Guid Id { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? DeviceId { get; set; }
        public Room Room { get; set; }
        public Device Device { get; set; }
        
    }
}
