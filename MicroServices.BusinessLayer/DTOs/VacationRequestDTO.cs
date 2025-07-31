using MicroServices.Models.Enums;

namespace MicroServices.BusinessLayer.DTOs
{
    public class VacationRequestDTO
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VacationStatus Status { get; set; } = VacationStatus.Pending;
    }
}