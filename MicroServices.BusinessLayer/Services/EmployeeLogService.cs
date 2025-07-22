using AutoMapper;
using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using MicroServices.DataAccessLayer.Interfaces;

namespace MicroServices.BusinessLayer.Services
{
    public class EmployeeLogService : IEmployeeLogService
    {
        private readonly IEmployeeLogDataAccess _dataAccess;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeLogService(IEmployeeLogDataAccess dataAccess, IEmployeeService employeeService, IMapper mapper)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
            _employeeService = employeeService;
            _mapper = mapper;
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

        public async Task<List<AttendanceDTO>> GetAttendanceByEmployeeIdAsync(Guid employeeId, EmployeeDTO employee)
        {
            var logs = await _dataAccess.GetLogsByEmployeeIdAsync(employeeId);
            var shiftList = await _dataAccess.GetShiftByEmployeeIdAsync(employee.ShiftId);
            var shift = shiftList.FirstOrDefault();

            if (shift == null)
                throw new InvalidOperationException($"No shift found for ShiftId: {employee.ShiftId}");

            if (string.IsNullOrWhiteSpace(shift.WorkStart) || string.IsNullOrWhiteSpace(shift.WorkEnd))
                throw new InvalidOperationException("Shift start or end time is missing.");

            TimeSpan shiftStart = TimeSpan.Parse(shift.WorkStart);
            TimeSpan shiftEnd = TimeSpan.Parse(shift.WorkEnd);

            DateTime today = DateTime.Today;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);

            var groupedByDay = logs
                .Where(l => l.Timestamp.Date >= startOfMonth && l.Timestamp.Date <= today)
                .GroupBy(l => l.Timestamp.Date)
                .OrderBy(g => g.Key);

            var result = new List<AttendanceDTO>();

            foreach (var dayLogs in groupedByDay)
            {
                var date = dayLogs.Key;

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var enterLog = dayLogs
                    .Where(l => l.Action.Equals("enter", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(l => l.Timestamp)
                    .FirstOrDefault();

                var exitLog = dayLogs
                    .Where(l => l.Action.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(l => l.Timestamp)
                    .FirstOrDefault();

                if (enterLog == null || exitLog == null)
                    continue;

                int minutesLeftEarly = 0;
                DateTime expectedStart = date + shiftStart;
                DateTime expectedEnd = date + shiftEnd;

                DateTime realStart = enterLog.Timestamp < expectedStart ? expectedStart : enterLog.Timestamp;
                DateTime realEnd = exitLog.Timestamp;

                if (exitLog.Timestamp <= expectedStart || enterLog.Timestamp >= expectedEnd)
                {
                    minutesLeftEarly = (int)(expectedEnd - expectedStart).TotalMinutes;
                }
                else
                {
                    DateTime actualExit = exitLog.Timestamp;
                    if (actualExit < expectedEnd)
                    {
                        minutesLeftEarly = (int)(expectedEnd - actualExit).TotalMinutes;
                    }
                }

                result.Add(new AttendanceDTO
                {
                    Employee = employee,
                    EmployeeShift = shift,
                    EarliestEnterTime = enterLog.Timestamp,
                    LatestExitTime = exitLog.Timestamp,
                    MinutesLate = minutesLeftEarly
                });
            }

            return result;
        }

        public async Task<List<List<AttendanceDTO>>> GetAttendancesByRolesAsync(List<int> roleIds)
        {
            var employees = _employeeService.GetAll();
            var filteredEmployees = employees.Where(e => roleIds.Contains(e.EmployeeRoleId)).ToList();

            var results = new List<List<AttendanceDTO>>();

            foreach (var emp in filteredEmployees)
            {
                var empDto = _mapper.Map<EmployeeDTO>(emp);
                var attendance = await GetAttendanceByEmployeeIdAsync(emp.Id, empDto);
                if (attendance != null && attendance.Any())
                    results.Add(attendance);
            }

            return results;
        }
    }
}