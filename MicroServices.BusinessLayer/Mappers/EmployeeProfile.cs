using AutoMapper;
using MicroServices.BusinessLayer.DTOs;
using MicroServices.Models;

namespace MicroServices.BusinessLayer.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.ShiftId, opt => opt.MapFrom(src => src.EmployeeShiftId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.EmployeeRoleId));

            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeRoleId, opt => opt.MapFrom(map => map.RoleId))
                .ForMember(dest => dest.EmployeeShiftId, opt => opt.MapFrom(map => map.ShiftId));
        }
    }
}