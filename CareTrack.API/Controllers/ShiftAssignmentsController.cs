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
    
    public class ShiftAssignmentsController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IShiftAssignmentRepository shiftAssignmentRepository;
        private readonly IEmployeeRepository employeeRepository;

        public ShiftAssignmentsController(IMapper mapper, IShiftAssignmentRepository shiftAssignmentRepository, IEmployeeRepository employeeRepository)
        {
            this.mapper = mapper;
            this.shiftAssignmentRepository = shiftAssignmentRepository;
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Create([FromBody] AddShiftAssignmentDto addShiftAssignmentDto)
        {
            //Map DTO to domain model
            var shiftAssignmentDomainModel = mapper.Map<ShiftAssignment>(addShiftAssignmentDto);

            await shiftAssignmentRepository.CreateAsync(shiftAssignmentDomainModel);

            //Map Domain Model to DTO
            var shiftAssignmentDto = mapper.Map<ShiftAssignmentDto>(shiftAssignmentDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = shiftAssignmentDto.Id }, shiftAssignmentDto);

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? filterQueryII, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("Error while identifying user");
            }

            if (!User.IsInRole("Super Admin") && !User.IsInRole("Admin"))
            {
                // User is not an admin, so we need to apply restrictions
                if (filterOn != "EmployeeId" || string.IsNullOrWhiteSpace(filterQuery))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Non-admin users must provide an EmployeeId filter.");
                }

                var employee = await employeeRepository.GetByUserIdAsync(Guid.Parse(userId));
                if (employee == null || employee.Id.ToString() != filterQuery)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Non-admin users can only access their own shift assignments.");
                }
            }

            // Get Data From database - Domain Models
            var shiftAssignmentsDomain = await shiftAssignmentRepository.GetAllAsync(filterOn, filterQuery, filterQueryII, sortBy, isAscending ?? true, pageNumber, pageSize);

            // Map Domain Models to DTOs
            var shiftAssignmentsDto = mapper.Map<List<ShiftAssignmentDto>>(shiftAssignmentsDomain);

            // Return DTOs
            return Ok(shiftAssignmentsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var shiftAssignmentDomainModel = await shiftAssignmentRepository.GetByIdAsync(id);

            if (shiftAssignmentDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<ShiftAssignmentDto>(shiftAssignmentDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateShiftAssignmentDto updateShiftAssignmentDto)
        {

            var shiftAssignmentDomainModel = mapper.Map<ShiftAssignment>(updateShiftAssignmentDto);
            shiftAssignmentDomainModel = await shiftAssignmentRepository.UpdateAsync(id, shiftAssignmentDomainModel);

            if (shiftAssignmentDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ShiftAssignmentDto>(shiftAssignmentDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var shiftAssignmentDomainModel = await shiftAssignmentRepository.DeleteAsync(id);

            if (shiftAssignmentDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ShiftAssignmentDto>(shiftAssignmentDomainModel));

        }
    }
}
