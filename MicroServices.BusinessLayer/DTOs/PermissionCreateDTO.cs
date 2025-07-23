
namespace MicroServices.BusinessLayer.DTOs
{
    public class PermissionCreateDTO
    {
        public Guid TargetEmployeeId { get; set; }
        public string Reason { get; set; }
        public DateTime BeginDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}