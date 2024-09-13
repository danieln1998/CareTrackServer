using CareTrack.API.Models.Domain;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class AlertDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public PatientDto Patient { get; set; }
    }
}
