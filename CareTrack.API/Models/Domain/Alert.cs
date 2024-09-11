namespace CareTrack.API.Models.Domain
{
    public class Alert
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
