using MicroServices.Models.Enums;

namespace MicroServices.Models

{
    public class Vacation
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VacationStatus Status { get; set; } = VacationStatus.Pending;
    }
}