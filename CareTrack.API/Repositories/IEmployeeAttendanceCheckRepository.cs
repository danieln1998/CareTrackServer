using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO;

namespace CareTrack.API.Repositories
{
    public interface IEmployeeAttendanceCheckRepository
    {
        Task EmployeeAttendanceCheckMain(Guid shiftId);
        Task<List<EmpolyeeIdDto>> GetEmployeesIdFromShiftAssignment(Guid id);
        Task<List<EmpolyeeIdDto>> CheckMissingEmployees(List<EmpolyeeIdDto> employeesId, Guid shiftId);
        Task MarkingMissing(List<EmpolyeeIdDto> missingEmployees, List<EmpolyeeIdDto> employeesId, Guid shiftId);
        Task TardinessCheck(Guid shiftId, Guid EmployeeId);
        TimeSpan CalculationOfTimeDifferenceForJob(Shift shift);
    }
}
