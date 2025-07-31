using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIPractice.Controllers
{
    [ApiController]
    [Route("api/vacation")]
    public class VacationController : ControllerBase
    {
        private readonly IVacationService _vacationService;

        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        [HttpGet("{employeeId}/left")]
        public async Task<IActionResult> GetLeft(Guid employeeId)
        {
            var requesterId = GetRequesterId();
            var left = await _vacationService.GetVacationsLeftAsync(requesterId, employeeId);
            return Ok(left);
        }

        [HttpPost("{employeeId}/request")]
        public async Task<IActionResult> Request(Guid employeeId, VacationRequestDTO dto)
        {
            var requesterId = GetRequesterId();
            await _vacationService.RequestVacationAsync(requesterId, employeeId, dto);
            return Ok("Vacation request created.");
        }

        [HttpPut("approve-or-reject")]
        public async Task<IActionResult> ApproveOrReject(VacationApprovalDTO dto)
        {
            var approverId = GetBossId();
            dto.ApproverId = approverId;
            await _vacationService.ApproveOrRejectAsync(dto);
            return Ok("Vacation status updated.");
        }

        [HttpGet("{employeeId}/requests")]
        public async Task<IActionResult> GetRequests(Guid employeeId)
        {
            var requesterId = GetRequesterId();
            var requests = await _vacationService.GetRequestsAsync(requesterId, employeeId);
            return Ok(requests);
        }

        private Guid GetRequesterId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID claim is missing.");
            }
            return Guid.Parse(userIdClaim);
        }

        private Guid GetBossId()
        {
            var bossIdClaim = User.FindFirst("BossId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(bossIdClaim))
            {
                throw new InvalidOperationException("Boss ID claim is missing.");
            }
            return Guid.Parse(bossIdClaim);
        }
    }
}