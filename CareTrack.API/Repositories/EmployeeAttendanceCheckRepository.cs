
using AutoMapper;
using CareTrack.API.Data;
using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CareTrack.API.Repositories
{
    public class EmployeeAttendanceCheckRepository : IEmployeeAttendanceCheckRepository
    {
        private readonly IMapper mapper;
        private readonly IShiftAssignmentRepository shiftAssignmentRepository;
        private readonly IEmployeeShiftRepository employeeShiftRepository;
        private readonly CareTrackDbcontext dbContext;

        public EmployeeAttendanceCheckRepository(IMapper mapper, IShiftAssignmentRepository shiftAssignmentRepository, 
            IEmployeeShiftRepository employeeShiftRepository , CareTrackDbcontext dbContext) {
            this.mapper = mapper;
            this.shiftAssignmentRepository = shiftAssignmentRepository;
            this.employeeShiftRepository = employeeShiftRepository;
            this.dbContext = dbContext;
        }

        public async Task EmployeeAttendanceCheckMain(Guid shiftId)
        {
            var employeesId = await GetEmployeesIdFromShiftAssignment(shiftId);
            if (employeesId.Count == 0)
            {
                return;
            }
            var missingEmployees = await CheckMissingEmployees(employeesId, shiftId);
            await MarkingMissing(missingEmployees,employeesId,shiftId);
        }
        public async Task<List<EmpolyeeIdDto>> GetEmployeesIdFromShiftAssignment(Guid shiftId)
        {
            var shiftAssignments = await shiftAssignmentRepository.GetAllAsync("ShiftId", shiftId.ToString());

            return mapper.Map<List<EmpolyeeIdDto>>(shiftAssignments);

        }

        public async Task<List<EmpolyeeIdDto>> CheckMissingEmployees(List<EmpolyeeIdDto> employeesId , Guid shiftId)
        {
        
            var employeeShifts = await employeeShiftRepository.GetAllAsync("ShiftId", shiftId.ToString());

            var existingEmployeesShift = mapper.Map<List<EmpolyeeIdDto>>(employeeShifts);
            List<EmpolyeeIdDto> missingEmployees = new List<EmpolyeeIdDto>();


            for (int i = 0; i < employeesId.Count; i++)
            {
                
                if (!existingEmployeesShift.Any(n => object.Equals(n.EmployeeId, employeesId[i].EmployeeId)))
                {
                    missingEmployees.Add(employeesId[i]);

                }
            }

            return missingEmployees;

        }

        public async Task MarkingMissing(List<EmpolyeeIdDto> missingEmployees, List<EmpolyeeIdDto> employeesId, Guid shiftId)
        {
            var shiftAssignments = await shiftAssignmentRepository.GetAllAsync("ShiftId", shiftId.ToString());
          
            for (int i = 0; i < shiftAssignments.Count; i++)
            {
                if (missingEmployees.Any(n => object.Equals(n.EmployeeId, employeesId[i].EmployeeId)))
                {
                    shiftAssignments[i].Status = "missing";
                }

            }
            await dbContext.SaveChangesAsync();

        }

        public async Task TardinessCheck(Guid shiftId,Guid EmployeeId)
        {
            var shiftAssignment = await shiftAssignmentRepository.GetAllAsync("ShiftId", shiftId.ToString(),EmployeeId.ToString());

            if (shiftAssignment[0].Status == "missing")
            {
                shiftAssignment[0].Status = "tardiness";
                await dbContext.SaveChangesAsync();
            }


        }

        public TimeSpan CalculationOfTimeDifferenceForJob(Shift shift)
        {
            DateTime startTime = shift.StartTime;
            DateTime currentTime = DateTime.Now;

            TimeSpan timeDifference = startTime.Subtract(currentTime);
            timeDifference += TimeSpan.FromMinutes(10);

            Console.WriteLine("Time Difference (minutes): " + timeDifference.TotalMinutes);

            return timeDifference;

        }

    }
}
