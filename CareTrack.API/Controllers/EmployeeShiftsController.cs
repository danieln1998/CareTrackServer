using AutoMapper;
using CareTrack.API.CustomActionFilters;
using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO;
using CareTrack.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeShiftsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEmployeeShiftRepository employeeShiftRepository;
        private readonly IEmployeeAttendanceCheckRepository employeeAttendanceCheckRepository;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeShiftsController(IMapper mapper, IEmployeeShiftRepository employeeShiftRepository, IEmployeeAttendanceCheckRepository employeeAttendanceCheckRepository, IEmployeeRepository employeeRepository)
        {
            this.mapper = mapper;
            this.employeeShiftRepository = employeeShiftRepository;
            this.employeeAttendanceCheckRepository = employeeAttendanceCheckRepository;
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddEmployeeShiftDto addEmployeeShiftDto)
        {
            //Map DTO to domain model
            var employeeShiftDomainModel = mapper.Map<EmployeeShift>(addEmployeeShiftDto);

            await employeeShiftRepository.CreateAsync(employeeShiftDomainModel);

            await employeeAttendanceCheckRepository.TardinessCheck(employeeShiftDomainModel.ShiftId, employeeShiftDomainModel.EmployeeId);
            //Map Domain Model to DTO
            var employeeShiftDto = mapper.Map<EmployeeShiftDto>(employeeShiftDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = employeeShiftDto.Id }, employeeShiftDto);

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? filterQueryII)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("Error while identifying user");
            }

            if (!User.IsInRole("Super Admin") && !User.IsInRole("Admin"))
            {
                // User is not an admin, so we need to apply restrictions
                if (filterOn != "ShiftId" || string.IsNullOrWhiteSpace(filterQuery) || string.IsNullOrWhiteSpace(filterQueryII))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Non-admin users must provide an Shift Id filter.");
                }

                var employee = await employeeRepository.GetByUserIdAsync(Guid.Parse(userId));
                if (employee == null || employee.Id.ToString() != filterQueryII)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Non-admin users can only access their own shift.");
                }
            }
            // Get Data From database - Domain Models

            var employeeShiftsDomain = await employeeShiftRepository.GetAllAsync(filterOn, filterQuery, filterQueryII);

            // Map Domain Models to DTOs
            var employeeShiftsDto = mapper.Map<List<EmployeeShiftDto>>(employeeShiftsDomain);

            // Return DTOs
            return Ok(employeeShiftsDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var employeeShiftDomainModel = await employeeShiftRepository.GetByIdAsync(id);

            if (employeeShiftDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<EmployeeShiftDto>(employeeShiftDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id)
        {

            var employeeShiftDomainModel = await employeeShiftRepository.UpdateAsync(id);

            if (employeeShiftDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<EmployeeShiftDto>(employeeShiftDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var employeeShiftDomainModel = await employeeShiftRepository.DeleteAsync(id);

            if (employeeShiftDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<EmployeeShiftDto>(employeeShiftDomainModel));

        }
    }
}
