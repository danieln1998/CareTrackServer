namespace CareTrack.API.Models.Domain
{
    public class Shift
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? JobId { get; set; }

    }
}
