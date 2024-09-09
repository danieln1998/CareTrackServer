using AutoMapper;
using CareTrack.API.CustomActionFilters;
using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO;
using CareTrack.API.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ShiftsController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IShiftRepository shiftRepository;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IEmployeeAttendanceCheckRepository employeeAttendanceCheckRepository;

        public ShiftsController(IMapper mapper, IShiftRepository shiftRepository, IBackgroundJobClient backgroundJobClient , IEmployeeAttendanceCheckRepository employeeAttendanceCheckRepository)
        {
            this.mapper = mapper;
            this.shiftRepository = shiftRepository;
            this.backgroundJobClient = backgroundJobClient;
            this.employeeAttendanceCheckRepository = employeeAttendanceCheckRepository;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Create([FromBody] AddShiftDto addShiftDto)
        {
            //Map DTO to domain model
            var shiftDomainModel = mapper.Map<Shift>(addShiftDto);

            TimeSpan timeDifference = employeeAttendanceCheckRepository.CalculationOfTimeDifferenceForJob(shiftDomainModel);
            
            if (timeDifference.TotalMinutes - 10 < 1 || shiftDomainModel.EndTime.Subtract(DateTime.Now).TotalMinutes < 10 || 
                shiftDomainModel.EndTime.Subtract(shiftDomainModel.StartTime).TotalMinutes <= 0 )
            {

                return BadRequest("Incorrect dates have been entered");

            }

            await shiftRepository.CreateAsync(shiftDomainModel);

            //Map Domain Model to DTO
            var shiftDto = mapper.Map<ShiftDto>(shiftDomainModel);


            var jobId = backgroundJobClient.Schedule(() =>
              employeeAttendanceCheckRepository.EmployeeAttendanceCheckMain(shiftDto.Id),
                TimeSpan.FromMinutes(timeDifference.TotalMinutes));

            await shiftRepository.SetJobId(shiftDto.Id, jobId);


            //await employeeAttendanceCheckRepository.EmployeeAttendanceCheckMain(Guid.Parse("5CEC2259-4800-4C67-D406-08DCA6793A44"));


            return CreatedAtAction(nameof(GetById), new { id = shiftDto.Id }, shiftDto);

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetAll([FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From database - Domain Models

            var shiftsDomain = await shiftRepository.GetAllAsync(sortBy, isAscending ?? true, pageNumber, pageSize);

            // Map Domain Models to DTOs
            var shiftsDto = mapper.Map<List<ShiftDto>>(shiftsDomain);

            // Return DTOs
            return Ok(shiftsDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var shiftDomainModel = await shiftRepository.GetByIdAsync(id);

            if (shiftDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<ShiftDto>(shiftDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateShiftDto updateShiftDto)
        {

            var shiftDomainModel = mapper.Map<Shift>(updateShiftDto);

            TimeSpan timeDifference = employeeAttendanceCheckRepository.CalculationOfTimeDifferenceForJob(shiftDomainModel);

            if (timeDifference.TotalMinutes - 10 < 1 || shiftDomainModel.EndTime.Subtract(DateTime.Now).TotalMinutes < 10 ||
               shiftDomainModel.EndTime.Subtract(shiftDomainModel.StartTime).TotalMinutes <= 0)
            {

                return BadRequest("Incorrect dates have been entered");

            }

            shiftDomainModel = await shiftRepository.UpdateAsync(id, shiftDomainModel);

            
            if (shiftDomainModel == null)
            {
                return NotFound();
            }

            backgroundJobClient.Delete(shiftDomainModel.JobId);

            
            var jobId = backgroundJobClient.Schedule(() =>
                employeeAttendanceCheckRepository.EmployeeAttendanceCheckMain(id),
                TimeSpan.FromMinutes(timeDifference.TotalMinutes));

            await shiftRepository.SetJobId(id, jobId);

            return Ok(mapper.Map<ShiftDto>(shiftDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var shiftDomainModel = await shiftRepository.DeleteAsync(id);

            if (shiftDomainModel == null)
            {
                return NotFound();
            }

            backgroundJobClient.Delete(shiftDomainModel.JobId);

            return Ok(mapper.Map<ShiftDto>(shiftDomainModel));

        }

        
    }
}
