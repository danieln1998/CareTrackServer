using CareTrack.API.Models.Domain;

namespace CareTrack.API.Models.DTO.PatientsDTO
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public RoomDto? Room { get; set; }
        public DeviceDto? Device { get; set; }
    }
}
