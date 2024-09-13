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
    public class PatientsController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IPatientRepository patientRepository;

        public PatientsController(IMapper mapper, IPatientRepository patientRepository)
        {
            this.mapper = mapper;
            this.patientRepository = patientRepository;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Create([FromBody] AddPatientDto addPatientDto)
        {
            //Map DTO to domain model
            var patientDomainModel = mapper.Map<Patient>(addPatientDto);

            await patientRepository.CreateAsync(patientDomainModel);

            //Map Domain Model to DTO
            var patientDto = mapper.Map<PatientDto>(patientDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = patientDto.Id }, patientDto);

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From database - Domain Models

            var patientsDomain = await patientRepository.GetAllAsync(pageNumber, pageSize);

            // Map Domain Models to DTOs
            var patientsDto = mapper.Map<List<PatientDto>>(patientsDomain);

            // Return DTOs
            return Ok(patientsDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var patientDomainModel = await patientRepository.GetByIdAsync(id);

            if (patientDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<PatientDto>(patientDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePatientDto updatePatientDto)
        {

            var patientDomainModel = mapper.Map<Patient>(updatePatientDto);
            patientDomainModel = await patientRepository.UpdateAsync(id, patientDomainModel);

            if (patientDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PatientDto>(patientDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var patientDomainModel = await patientRepository.DeleteAsync(id);

            if (patientDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PatientDto>(patientDomainModel));

        }
    }
}
