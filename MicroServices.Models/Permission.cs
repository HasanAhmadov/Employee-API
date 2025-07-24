using MicroServices.Models.Enums;

namespace MicroServices.Models
{
    public class Permission
    {
        public Guid Id { get; set; }
        public Guid RequesterId { get; set; }
        public Guid TargetEmployeeId { get; set; }
        public string Reason { get; set; }
        public DateTime BeginDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public PermissionStatus Status { get; set; } = PermissionStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}