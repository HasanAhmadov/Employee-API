
using MicroServices.Models.Enums;

namespace MicroServices.BusinessLayer.DTOs
{
    public class VacationApprovalDTO
    {
        public Guid VacationRequestId { get; set; }
        public VacationStatus Status { get; set; }
        public Guid ApproverId { get; set; }
    }
}