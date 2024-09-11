using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Models.Domain
{
    [Index(nameof(RoomNumber), IsUnique = true)]
    public class Room
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        
    }

}
