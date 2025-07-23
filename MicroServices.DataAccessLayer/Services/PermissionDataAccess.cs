using MicroServices.DataAccessLayer.Data;
using MicroServices.DataAccessLayer.Interfaces;
using MicroServices.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.DataAccessLayer.Services
{
    public class PermissionDataAccess : IPermissionDataAccess
    {
        private readonly ApplicationDbContext _context;

        public PermissionDataAccess(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<Permission?> GetByIdAsync(Guid id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task UpdateAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Permission>> GetByRequesterIdAsync(Guid requesterId)
        {
            return await Task.FromResult(_context.Permissions
                .Where(p => p.RequesterId == requesterId)
                .ToList());
        }

        public async Task<List<Permission>> GetByTargetEmployeeIdAsync(Guid targetEmployeeId)
        {
            return await Task.FromResult(_context.Permissions
                .Where(p => p.TargetEmployeeId == targetEmployeeId)
                .ToList());
        }

        public async Task<List<Permission>> GetAllRelatedToEmployeeAsync(Guid employeeId)
        {
            return await _context.Permissions
                .Where(p => p.RequesterId == employeeId || p.TargetEmployeeId == employeeId)
                .ToListAsync();
        }

    }
}