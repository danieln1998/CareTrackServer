using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Models.Domain
{
    [Index(nameof(DeviceNumber), IsUnique = true)]
    public class Device
    {
        public Guid Id { get; set; }
        public int DeviceNumber { get; set; }
        
    }
}
