using AutoMapper;
using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace APIPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeLogController : ControllerBase
    {
        private readonly IEmployeeLogService _employeeLogService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        private readonly IDbConnection db;
        public EmployeeLogController(IMapper mapper,IEmployeeLogService employeeLogService, IEmployeeService employeeService)
        {
            _employeeLogService = employeeLogService;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("Log")]
        public async Task<IActionResult> LogEntry([FromBody] EmployeeLogRequestDTO request)
        {
            
            var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(employeeId) || !Guid.TryParse(employeeId, out var guidEmployeeId))
            {
                return Unauthorized("Invalid employee ID in token.");
            }

            var result = await _employeeLogService.LogEntryAsync(guidEmployeeId, request);
            if (result == "Employee not found.")
                return NotFound(result);

            return Ok(result);
        }

        [Authorize(Roles = "1")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _employeeLogService.GetAllLogsAsync();
            return Ok(logs);
        }

        [Authorize]
        [HttpGet("GetByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetLogsByEmployeeId(Guid employeeId)
        {
            var currentEmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (!Guid.TryParse(currentEmployeeId, out var guidCurrentEmployeeId))
                return Forbid();

            if (currentUserRole == "1")
            {
                var logs = await _employeeLogService.GetLogsByEmployeeIdAsync(employeeId);
                return Ok(logs);
            }

            if (guidCurrentEmployeeId == employeeId)
            {
                var logs = await _employeeLogService.GetLogsByEmployeeIdAsync(employeeId);
                return Ok(logs);
            }

            var targetEmployee = _employeeService.GetById(employeeId);
            if (targetEmployee == null)
                return NotFound("Employee not found");

            var targetRole = targetEmployee.EmployeeRoleId.ToString();

            if ((currentUserRole == "2" && targetRole == "5") ||
                (currentUserRole == "4" && targetRole == "7") ||
                (currentUserRole == "3" && targetRole == "6"))
            {
                var logs = await _employeeLogService.GetLogsByEmployeeIdAsync(employeeId);
                return Ok(logs);
            }

            return Forbid();
        }

        [Authorize]
        [HttpGet("GetAttendanceByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetAttendanceByEmployeeId(Guid employeeId)
        {
            var currentEmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            var targetEmployee = _employeeService.GetById(employeeId);
            if (targetEmployee == null)
                return NotFound("Employee not found");

            var employeeDto = _mapper.Map<EmployeeDTO>(targetEmployee);

            if (!Guid.TryParse(currentEmployeeId, out var guidCurrentEmployeeId))
                return Forbid();

            if (currentUserRole == "1")
            {
                var attendance = await _employeeLogService.GetAttendanceByEmployeeIdAsync(employeeId, employeeDto);
                return Ok(attendance);
            }

            if (guidCurrentEmployeeId == employeeId)
            {
                var attendance = await _employeeLogService.GetAttendanceByEmployeeIdAsync(employeeId, employeeDto);
                return Ok(attendance);
            }

            var targetRole = targetEmployee.EmployeeRoleId.ToString();

            if ((currentUserRole == "2" && targetRole == "5") ||
                (currentUserRole == "4" && targetRole == "7") ||
                (currentUserRole == "3" && targetRole == "6"))
            {
                var attendance = await _employeeLogService.GetAttendanceByEmployeeIdAsync(employeeId, employeeDto);
                return Ok(attendance);
            }

            return Forbid();
        }
    }
}