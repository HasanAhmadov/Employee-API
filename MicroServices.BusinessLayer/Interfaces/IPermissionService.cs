using MicroServices.Models;

namespace MicroServices.BusinessLayer.Interfaces
{
    public interface IPermissionService
    {
        Task CreateAsync(Permission permission);
        Task<Permission?> GetByIdAsync(Guid id);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(Guid id);
        Task<List<Permission>> GetByRequesterIdAsync(Guid requesterId);
        Task<List<Permission>> GetByTargetEmployeeIdAsync(Guid targetEmployeeId);
        Task<List<Permission>> GetAllRelatedToEmployeeAsync(Guid employeeId);
    }
}