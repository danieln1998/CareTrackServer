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
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IMapper mapper , IEmployeeRepository employeeRepository)
        {
            this.mapper = mapper;
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Create([FromBody] AddEmployeeRequestDto addEmployeeRequestDto)
        {
            //Map DTO to domain model
            var employeeDomainModel = mapper.Map<Employee>(addEmployeeRequestDto);

            await employeeRepository.CreateAsync(employeeDomainModel);

            //Map Domain Model to DTO
            var employeeDto = mapper.Map<EmployeeDto>(employeeDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = employeeDto.Id }, employeeDto);

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From database - Domain Models

            var employeesDomain = await employeeRepository.GetAllAsync(pageNumber, pageSize);

            // Map Domain Models to DTOs
            var employeesDto = mapper.Map<List<EmployeeDto>>(employeesDomain);

            // Return DTOs
            return Ok(employeesDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var employeeDomainModel = await employeeRepository.GetByIdAsync(id);

            if (employeeDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<EmployeeDto>(employeeDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEmployeeRequestDto updateEmployeeRequestDto)
        {

            var employeeDomainModel = mapper.Map<Employee>(updateEmployeeRequestDto);
            employeeDomainModel = await employeeRepository.UpdateAsync(id, employeeDomainModel);

            if (employeeDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<EmployeeDto>(employeeDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var employeeDomainModel = await employeeRepository.DeleteAsync(id);

            if (employeeDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<EmployeeDto>(employeeDomainModel));

        }

        [HttpGet]
        [Route("GetEmployeeByUserId")]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> getEmployeeIdByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("Error while identifying user");
            }

            var employee = await employeeRepository.GetByUserIdAsync(Guid.Parse(userId));

            if (employee == null)
            {

                return NotFound();
            }

            var EmployeeId = employee.Id.ToString();

            return Ok(new {EmployeeId});
        }



    }
}
