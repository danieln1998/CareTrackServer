using AutoMapper;
using CareTrack.API.CustomActionFilters;
using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO.PatientsDTO;
using CareTrack.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAlertRepository alertRepository;

        public AlertsController(IMapper mapper, IAlertRepository alertRepository)
        {
            this.mapper = mapper;
            this.alertRepository = alertRepository;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> Create([FromBody] AddAlertDto addAlertDto)
        {
            //Map DTO to domain model
            var alertDomainModel = mapper.Map<Alert>(addAlertDto);

            await alertRepository.CreateAsync(alertDomainModel);

            //Map Domain Model to DTO
            var alertDto = mapper.Map<AlertDto>(alertDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = alertDto.Id }, alertDto);

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From database - Domain Models

            var alertsDomain = await alertRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // Map Domain Models to DTOs
            var alertsDto = mapper.Map<List<AlertDto>>(alertsDomain);

            // Return DTOs
            return Ok(alertsDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var alertDomainModel = await alertRepository.GetByIdAsync(id);

            if (alertDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<AlertDto>(alertDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAlertDto updateAlertDto)
        {

            var alertDomainModel = mapper.Map<Alert>(updateAlertDto);
            alertDomainModel = await alertRepository.UpdateAsync(id, alertDomainModel);

            if (alertDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<AlertDto>(alertDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var alertDomainModel = await alertRepository.DeleteAsync(id);

            if (alertDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<AlertDto>(alertDomainModel));

        }

    }
}
