namespace CareTrack.API.Models.DTO
{
    public class ShiftDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
