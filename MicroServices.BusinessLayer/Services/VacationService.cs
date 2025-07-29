using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using MicroServices.DataAccessLayer.Data;
using MicroServices.Models;
using MicroServices.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.BusinessLayer.Services
{
    public class VacationService : IVacationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeService _employeeService;

        public VacationService(ApplicationDbContext context, IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        public async Task<double> GetVacationsLeftAsync(Guid requesterId, Guid employeeId)
        {
            var today = DateTime.UtcNow.Date;
            var dayOfYear = today.DayOfYear;
            var accrued = 30.0 * dayOfYear / 365.0;

            var requester = _employeeService.GetById(requesterId);
            var targetEmployee = _employeeService.GetById(employeeId);
            if (requester == null || targetEmployee == null)
                return 0.00;

            if (requesterId != employeeId && !CanApprove(requester.EmployeeRoleId, targetEmployee.EmployeeRoleId))
                throw new UnauthorizedAccessException("You are not allowed to view this employee’s vacation info.");

            var approvedRequests = await _context.VacationRequests
                .Where(v => v.EmployeeId == targetEmployee.Id && v.Status == VacationStatus.Approved)
                .ToListAsync();

            var usedDays = approvedRequests.Sum(r => CountWeekdays(r.StartDate, r.EndDate));

            return accrued - usedDays;
        }

        public async Task RequestVacationAsync(Guid requesterId, Guid employeeId, VacationRequestDTO dto)
        {
            var vacationDays = CountWeekdays(dto.StartDate, dto.EndDate);
            var left = await GetVacationsLeftAsync(requesterId, employeeId);

            if (vacationDays > left)
                throw new Exception("Not enough vacation days left");

            var request = new Vacation
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = VacationStatus.Pending
            };

            _context.VacationRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveOrRejectAsync(VacationApprovalDTO dto)
        {
            var request = await _context.VacationRequests.FindAsync(dto.VacationRequestId);
            if (request == null)
                throw new Exception("Vacation request not found");

            if (dto.Status == VacationStatus.Pending)
                throw new Exception("Invalid status");

            var approver = await _context.Employees.FindAsync(dto.ApproverId);
            if (approver == null)
                throw new Exception("Approver not found");

            var requestOwner = await _context.Employees.FindAsync(request.EmployeeId);
            if (requestOwner == null)
                throw new Exception("Request owner not found");

            if (!CanApprove(approver.EmployeeRoleId, requestOwner.EmployeeRoleId))
                throw new Exception("You are not authorized to approve this request");

            request.Status = dto.Status;
            await _context.SaveChangesAsync();
        }

        private bool CanApprove(int approverRole, int targetRole)
        {
            return approverRole switch
            {
                1 => targetRole is 2 or 3 or 4,
                2 => targetRole == 5,
                3 => targetRole == 6,
                4 => targetRole == 7,
                _ => false
            };
        }

        private int CountWeekdays(DateTime start, DateTime end)
        {
            int totalDays = 0;
            for (var date = start.Date; date < end.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    totalDays++;
            }
            return totalDays;
        }
    }
}