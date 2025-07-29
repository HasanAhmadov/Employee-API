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
        private readonly IVacationService _service;

        public VacationController(IVacationService service)
        {
            _service = service;
        }

        [HttpGet("{employeeId}/left")]
        public async Task<IActionResult> GetLeft(Guid employeeId)
        {
            var requesterId = GetRequesterId();
            var left = await _service.GetVacationsLeftAsync(requesterId, employeeId);
            return Ok(left);
        }

        [HttpPost("{employeeId}/request")]
        public async Task<IActionResult> Request(Guid employeeId, VacationRequestDTO dto)
        {
            var requesterId = GetRequesterId();
            await _service.RequestVacationAsync(requesterId, employeeId, dto);
            return Ok("Vacation request created.");
        }

        [HttpPut("approve-or-reject")]
        public async Task<IActionResult> ApproveOrReject(VacationApprovalDTO dto)
        {
            await _service.ApproveOrRejectAsync(dto);
            return Ok("Vacation status updated.");
        }

        private Guid GetRequesterId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}