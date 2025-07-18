using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using MicroServices.DataAccessLayer.Interfaces;

namespace MicroServices.BusinessLayer.Services
{
    public class EmployeeLogService : IEmployeeLogService
    {
        private readonly IEmployeeLogDataAccess _dataAccess;

        public EmployeeLogService(IEmployeeLogDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task<string> LogEntryAsync(Guid employeeId, EmployeeLogRequestDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _dataAccess.LogEmployeeActionAsync(employeeId, request.Action)
                ? "Log recorded."
                : "Employee not found.";
        }

        public async Task<IEnumerable<object>> GetAllLogsAsync()
        {
            var logs = await _dataAccess.GetAllLogsAsync();

            return logs.Select(l => new
            {
                l.Employee?.Name,
                l.Employee?.Email,
                l.Action,
                Date = l.Timestamp.ToString("yyyy-MM-dd"),
                Time = l.Timestamp.ToString("HH:mm:ss")
            });
        }

        public async Task<IEnumerable<object>> GetLogsByEmployeeIdAsync(Guid employeeId)
        {
            var logs = await _dataAccess.GetLogsByEmployeeIdAsync(employeeId);

            return logs.Select(l => new
            {
                l.Employee?.Name,
                l.Employee?.Email,
                l.Action,
                Date = l.Timestamp.ToString("yyyy-MM-dd"),
                Time = l.Timestamp.ToString("HH:mm:ss")
            });
        }
    }
}