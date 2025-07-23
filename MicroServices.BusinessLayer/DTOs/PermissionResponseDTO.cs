using MicroServices.Models.Enums;

namespace MicroServices.BusinessLayer.DTOs
{
    public class PermissionResponseDTO
    {
        public Guid Id { get; set; }
        public string RequesterName { get; set; }
        public string TargetEmployeeName { get; set; }
        public string Reason { get; set; }
        public DateTime BeginDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public PermissionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}