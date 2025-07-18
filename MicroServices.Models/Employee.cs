using System.ComponentModel.DataAnnotations.Schema;

namespace MicroServices.Models
{
    public class Employee
    {
        public Guid Id {  get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone {  get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public required string PasswordHash { get; set; }
        public int EmployeeRoleId { get; set; }
        public required Role EmployeeRole { get; set; }
        public required Guid BossId { get; set; }
    }
}