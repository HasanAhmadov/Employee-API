using MicroServices.BusinessLayer.Interfaces;
using MicroServices.DataAccessLayer.Interfaces;
using MicroServices.Models;

namespace MicroServices.BusinessLayer.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionDataAccess _permissionDataAccess;

        public PermissionService(IPermissionDataAccess permissionDataAccess)
        {
            _permissionDataAccess = permissionDataAccess;
        }

        public async Task CreateAsync(Permission permission)
        {
            await _permissionDataAccess.CreateAsync(permission);
        }

        public async Task<Permission?> GetByIdAsync(Guid id)
        {
            return await _permissionDataAccess.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Permission permission)
        {
            await _permissionDataAccess.UpdateAsync(permission);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _permissionDataAccess.DeleteAsync(id);
        }

        public async Task<List<Permission>> GetByRequesterIdAsync(Guid requesterId)
        {
            return await _permissionDataAccess.GetByRequesterIdAsync(requesterId);
        }

        public async Task<List<Permission>> GetByTargetEmployeeIdAsync(Guid targetEmployeeId)
        {
            return await _permissionDataAccess.GetByTargetEmployeeIdAsync(targetEmployeeId);
        }

        public async Task<List<Permission>> GetAllRelatedToEmployeeAsync(Guid employeeId)
        {
            return await _permissionDataAccess.GetAllRelatedToEmployeeAsync(employeeId);
        }

    }
}