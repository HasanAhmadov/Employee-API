using MicroServices.Models;

namespace MicroServices.DataAccessLayer.Interfaces
{
    public interface IEmployeeDataAccess
    {
        List<Employee> GetAll();
        Employee? GetById(Guid id);
        Employee Add(Employee employee);
        Employee? Update(Guid id, Employee employee);
        bool Delete(Guid id);
        Role GetRoleById(int id);
        Guid GetBossIdById(Guid id);
    }
}