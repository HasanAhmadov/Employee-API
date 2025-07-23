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
            var bossId = employeeDataAccess.GetBossIdById(dto.BossId);
            var shift = employeeDataAccess.GetShiftById(dto.ShiftId);

            entity.EmployeeRoleId = dto.RoleId;
            entity.EmployeeRole = role;
            entity.BossId = bossId;
            entity.EmployeeShiftId = dto.ShiftId;
            entity.EmployeeShift = shift;

            return employeeDataAccess.Add(entity);
        }

        public Employee? Update(Guid id, EmployeeDTO dto)
        {
            var updatedEntity = _mapper.Map<Employee>(dto);

            updatedEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var response = employeeDataAccess.Update(id, updatedEntity);

            return response;
        }

        public bool Delete(Guid id)
        {
            return employeeDataAccess.Delete(id);
        }

        public Guid GetBossOfEmployeeAsync(Guid employeeId)
        {
            var employee = employeeDataAccess.GetById(employeeId);
            if (employee == null)
                throw new Exception("There's no such that employee!");

            return employee.BossId;
        }

    }
}