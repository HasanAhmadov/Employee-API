using MicroServices.BusinessLayer.DTOs;
using MicroServices.Models;

namespace MicroServices.BusinessLayer.Interfaces
{
    public interface IVacationService
    {
        Task<double> GetVacationsLeftAsync(Guid requesterId, Guid employeeId);
        Task RequestVacationAsync(Guid requesterId, Guid employeeId, VacationRequestDTO dto);
        Task ApproveOrRejectAsync(VacationApprovalDTO dto);
        Task<List<VacationRequestDTO>> GetRequestsAsync(Guid requesterId, Guid employeeId);
    }
}