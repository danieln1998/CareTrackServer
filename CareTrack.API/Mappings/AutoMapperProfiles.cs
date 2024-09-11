using CareTrack.API.Models.Domain;
using CareTrack.API.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using CareTrack.API.Models.DTO.PatientsDTO;

namespace CareTrack.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<AddEmployeeRequestDto, Employee>().ReverseMap();
            CreateMap<UpdateEmployeeRequestDto, Employee>().ReverseMap();
           
            CreateMap<Shift, ShiftDto>().ReverseMap();
            CreateMap<AddShiftDto, Shift>().ReverseMap();
            CreateMap<UpdateShiftDto, Shift>().ReverseMap();

            CreateMap<ShiftAssignment, ShiftAssignmentDto>().ReverseMap();
            CreateMap<AddShiftAssignmentDto, ShiftAssignment>().ReverseMap();
            CreateMap<UpdateShiftAssignmentDto, ShiftAssignment>().ReverseMap();

            CreateMap<EmployeeShift, EmployeeShiftDto>().ReverseMap();
            CreateMap<AddEmployeeShiftDto, EmployeeShift>().ReverseMap();

            CreateMap<ShiftAssignment, EmpolyeeIdDto>().ReverseMap();
            CreateMap<EmployeeShift, EmpolyeeIdDto>().ReverseMap();

            CreateMap<IdentityUser, UserDto>().ReverseMap();

            // Patients Mapping

            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<AddRoomDto, Room>().ReverseMap();
            CreateMap<UpdateRoomDto, Room>().ReverseMap();

            CreateMap<Device, DeviceDto>().ReverseMap();
            CreateMap<AddDeviceDto, Device>().ReverseMap();
            CreateMap<UpdateDeviceDto, Device>().ReverseMap();

        }
    }
}
