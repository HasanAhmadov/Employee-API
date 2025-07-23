using MicroServices.BusinessLayer.DTOs;
using MicroServices.Models;

namespace MicroServices.BusinessLayer.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        Employee? GetById(Guid id);
        Employee Add(EmployeeDTO dto);
        Employee? Update(Guid id, EmployeeDTO dto);
        bool Delete(Guid id);
        Guid GetBossOfEmployeeAsync(Guid employeeId);
    }
}