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
    public class DevicesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDeviceRepository deviceRepository;

        public DevicesController(IMapper mapper, IDeviceRepository deviceRepository)
        {
            this.mapper = mapper;
            this.deviceRepository = deviceRepository;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Create([FromBody] AddDeviceDto addDeviceDto)
        {
            //Map DTO to domain model
            var deviceDomainModel = mapper.Map<Device>(addDeviceDto);

            await deviceRepository.CreateAsync(deviceDomainModel);

            //Map Domain Model to DTO
            var deviceDto = mapper.Map<DeviceDto>(deviceDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = deviceDto.Id }, deviceDto);

        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From database - Domain Models

            var devicesDomain = await deviceRepository.GetAllAsync(pageNumber, pageSize);

            // Map Domain Models to DTOs
            var devicesDto = mapper.Map<List<DeviceDto>>(devicesDomain);

            // Return DTOs
            return Ok(devicesDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var deviceDomainModel = await deviceRepository.GetByIdAsync(id);

            if (deviceDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<DeviceDto>(deviceDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDeviceDto updateDeviceDto)
        {

            var deviceDomainModel = mapper.Map<Device>(updateDeviceDto);
            deviceDomainModel = await deviceRepository.UpdateAsync(id, deviceDomainModel);

            if (deviceDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<DeviceDto>(deviceDomainModel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deviceDomainModel = await deviceRepository.DeleteAsync(id);

            if (deviceDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<DeviceDto>(deviceDomainModel));

        }
    }
}
