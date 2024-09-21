using AutoMapper;
using CareTrack.API.CustomActionFilters;
using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO;
using CareTrack.API.Models.DTO.PatientsDTO;
using CareTrack.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CareTrack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRoomRepository roomRepository;

        public RoomsController(IMapper mapper, IRoomRepository roomRepository)
        {
            this.mapper = mapper;
            this.roomRepository = roomRepository;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Create([FromBody] AddRoomDto addRoomDto)
        {
            try
            {
                //Map DTO to domain model
                var roomDomainModel = mapper.Map<Room>(addRoomDto);

                await roomRepository.CreateAsync(roomDomainModel);

                //Map Domain Model to DTO
                var roomDto = mapper.Map<RoomDto>(roomDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = roomDto.Id }, roomDto);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    return Conflict("The room number already exists in the system. Please choose another number.");
                }
                return StatusCode(500, "An error occurred while adding the room. Please try again later.");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get Data From database - Domain Models

            var roomsDomain = await roomRepository.GetAllAsync(pageNumber, pageSize);

            // Map Domain Models to DTOs
            var roomsDto = mapper.Map<List<RoomDto>>(roomsDomain);

            // Return DTOs
            return Ok(roomsDto);


        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin,Admin,User")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var roomDomainModel = await roomRepository.GetByIdAsync(id);

            if (roomDomainModel == null)
            {

                return NotFound();
            }

            return Ok(mapper.Map<RoomDto>(roomDomainModel));
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoomDto updateRoomDto)
        {
            try
            {
                var roomDomainModel = mapper.Map<Room>(updateRoomDto);
                roomDomainModel = await roomRepository.UpdateAsync(id, roomDomainModel);

                if (roomDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<RoomDto>(roomDomainModel));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    return Conflict("The room number already exists in the system. Please choose another number.");
                }
                return StatusCode(500, "An error occurred while update the room. Please try again later.");
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var roomDomainModel = await roomRepository.DeleteAsync(id);

            if (roomDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RoomDto>(roomDomainModel));

        }

        
    }
}
