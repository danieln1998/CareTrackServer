using AutoMapper;
using CareTrack.API.CustomActionFilters;
using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO.PatientsDTO;
using CareTrack.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                //Map DTO to domain model
                var deviceDomainModel = mapper.Map<Device>(addDeviceDto);

                await deviceRepository.CreateAsync(deviceDomainModel);

                //Map Domain Model to DTO
                var deviceDto = mapper.Map<DeviceDto>(deviceDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = deviceDto.Id }, deviceDto);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    return Conflict("The device number already exists in the system. Please choose another number.");
                }
                return StatusCode(500, "An error occurred while adding the device. Please try again later.");
            }
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
            try
            {
                var deviceDomainModel = mapper.Map<Device>(updateDeviceDto);
                deviceDomainModel = await deviceRepository.UpdateAsync(id, deviceDomainModel);

                if (deviceDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<DeviceDto>(deviceDomainModel));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    return Conflict("The device number already exists in the system. Please choose another number.");
                }
                return StatusCode(500, "An error occurred while update the device. Please try again later.");
            }
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
