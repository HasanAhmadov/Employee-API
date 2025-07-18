using MicroServices.Models;

namespace MicroServices.DataAccessLayer.Interfaces
{
    public interface IEmployeeLogDataAccess
    {
        Task<bool> LogEmployeeActionAsync(Guid employeeId, string action);
        Task<IEnumerable<EmployeeLog>> GetAllLogsAsync();
        Task<IEnumerable<EmployeeLog>> GetLogsByEmployeeIdAsync(Guid employeeId);
    }
}
