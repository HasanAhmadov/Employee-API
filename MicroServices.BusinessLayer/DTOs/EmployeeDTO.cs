using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServices.BusinessLayer.DTOs
{
    public class EmployeeDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public required string Password { get; set; }
        public required int RoleId { get; set; }
        public required Guid BossId { get; set; }
    }
}