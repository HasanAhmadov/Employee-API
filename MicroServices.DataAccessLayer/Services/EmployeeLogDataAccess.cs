using MicroServices.DataAccessLayer.Data;
using MicroServices.DataAccessLayer.Interfaces;
using MicroServices.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.DataAccessLayer.Services
{
    public class EmployeeLogDataAccess : IEmployeeLogDataAccess
    {
        private readonly ApplicationDbContext _context;

        public EmployeeLogDataAccess(ApplicationDbContext context)
            => _context = context;

        public async Task<bool> LogEmployeeActionAsync(Guid employeeId, string action)
        {
            if (!await _context.Employees.AnyAsync(e => e.Id == employeeId))
                return false; 

            _context.EmployeeLogs.Add(new EmployeeLog
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                Action = action,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeLog>> GetAllLogsAsync()
            => await _context.EmployeeLogs
                .Include(l => l.Employee)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();

        public async Task<IEnumerable<EmployeeLog>> GetLogsByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeLogs
                .Include(l => l.Employee)
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }
    }
}