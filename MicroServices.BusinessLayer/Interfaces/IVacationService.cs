using MicroServices.BusinessLayer.DTOs;

namespace MicroServices.BusinessLayer.Interfaces
{
    public interface IVacationService
    {
        Task<double> GetVacationsLeftAsync(Guid requesterId, Guid employeeId);
        Task RequestVacationAsync(Guid requesterId, Guid employeeId, VacationRequestDTO dto);
        Task ApproveOrRejectAsync(VacationApprovalDTO dto);
    }
}