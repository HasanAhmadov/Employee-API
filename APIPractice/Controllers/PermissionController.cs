using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using MicroServices.Models;
using MicroServices.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIPractice.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IEmployeeService _employeeService;

        public PermissionController(IPermissionService permissionService, IEmployeeService employeeService)
        {
            _permissionService = permissionService;
            _employeeService = employeeService;
        }

        [HttpPost("RequestToBoss")]
        public async Task<IActionResult> RequestToBoss([FromBody] PermissionCreateDTO dto)
        {
            var requesterIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(requesterIdStr, out Guid requesterId))
                return Forbid();

            var employee = _employeeService.GetById(requesterId);
            if (employee == null)
                return NotFound("Employee not found.");

            var boss = _employeeService.GetById(employee.BossId);
            if (boss == null)
                return NotFound("Boss not found.");

            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                RequesterId = requesterId,
                TargetEmployeeId = boss.Id,
                Reason = dto.Reason,
                BeginDate = dto.BeginDate,
                EndDate = dto.EndDate,
                Status = PermissionStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };

            await _permissionService.CreateAsync(permission);
            return Ok(permission);
        }

        [HttpPost("CreateForEmployee")]
        public async Task<IActionResult> CreateForEmployee([FromBody] PermissionCreateDTO dto)
        {
            var bossIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(bossIdStr, out Guid bossId))
                return Forbid();

            var boss = _employeeService.GetById(bossId);
            var employee = _employeeService.GetById(dto.TargetEmployeeId);

            if (boss == null || employee == null)
                return NotFound("Boss or employee not found.");

            if (employee.BossId != bossId)
                return Forbid("You are not the boss of this employee.");

            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                RequesterId = bossId,
                TargetEmployeeId = dto.TargetEmployeeId,
                Reason = dto.Reason,
                BeginDate = dto.BeginDate,
                EndDate = dto.EndDate,
                Status = PermissionStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };

            await _permissionService.CreateAsync(permission);
            return Ok(permission);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ApproveOrReject(Guid id, [FromQuery] string status)
        {
            var permission = await _permissionService.GetByIdAsync(id);
            if (permission == null)
                return NotFound("Permission not found.");

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out Guid approverId))
                return Forbid();

            if (permission.RequesterId == approverId)
                return BadRequest("You cannot approve or reject your own request.");

            var requester = _employeeService.GetById(permission.RequesterId);
            if (requester == null || requester.BossId != approverId)
                return Forbid("You are not authorized to approve or reject this request.");

            if (!Enum.TryParse<PermissionStatus>(status, true, out var parsedStatus) ||
                !Enum.IsDefined(typeof(PermissionStatus), parsedStatus) ||
                parsedStatus == PermissionStatus.Pending)
            {
                return BadRequest("Status must be either 'Approved' or 'Rejected'.");
            }

            if (permission.Status != PermissionStatus.Pending)
            {
                return BadRequest("This permission has already been processed.");
            }

            permission.Status = parsedStatus;
            await _permissionService.UpdateAsync(permission);

            return Ok("Permission status updated.");
        }

        [HttpGet("MyPermissions")]
        public async Task<IActionResult> GetMyPermissions()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return Forbid();

            var permissions = await _permissionService.GetAllRelatedToEmployeeAsync(userId);

            var orderedPermissions = permissions
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return Ok(orderedPermissions);
        }
    }
}