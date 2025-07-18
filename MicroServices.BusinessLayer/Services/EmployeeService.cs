using AutoMapper;
using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using MicroServices.DataAccessLayer.Interfaces;
using MicroServices.Models;

namespace MicroServices.BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDataAccess employeeDataAccess;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeDataAccess employeeDataAccess, IMapper mapper)
        {
            this.employeeDataAccess = employeeDataAccess;
            this._mapper = mapper;
        }

        public List<Employee> GetAll()
        {
            return employeeDataAccess.GetAll(); 
        }

        public Employee? GetById(Guid id)
        {
            return employeeDataAccess.GetById(id); 
        }

        public Employee Add(EmployeeDTO dto)
        {
            var entity = _mapper.Map<Employee>(dto);

            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var role = employeeDataAccess.GetRoleById(dto.RoleId);
            entity.EmployeeRoleId = dto.RoleId;
            entity.EmployeeRole = role;

            return employeeDataAccess.Add(entity);
        }

        public Employee? Update(Guid id, EmployeeDTO dto)
        {
            var updatedEntity = _mapper.Map<Employee>(dto);

            updatedEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            return employeeDataAccess.Update(id, updatedEntity);
        }

        public bool Delete(Guid id)
        {
            return employeeDataAccess.Delete(id);
        }
    }
}