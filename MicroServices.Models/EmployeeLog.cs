
namespace MicroServices.Models
{
    public class EmployeeLog
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } 
        public Employee Employee { get; set; } = null!;
    }
}