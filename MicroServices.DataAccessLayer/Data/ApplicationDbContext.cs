using MicroServices.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options){}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLog> EmployeeLogs { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}