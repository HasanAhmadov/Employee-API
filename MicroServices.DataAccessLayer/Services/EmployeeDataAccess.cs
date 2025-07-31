using MicroServices.DataAccessLayer.Data;
using MicroServices.DataAccessLayer.Interfaces;
using MicroServices.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.DataAccessLayer.Services
{
    public class EmployeeDataAccess(ApplicationDbContext dbContext) : IEmployeeDataAccess
    {
        private readonly ApplicationDbContext dbContext = dbContext;

        public List<Employee> GetAll()
        {
            var employees = dbContext.Employees
            .Include(e => e.EmployeeRole)
            .Include(e => e.EmployeeShift)
            .ToList();
            return [.. employees];
        }

        public Employee? GetById(Guid id)
        {
            return dbContext.Employees
                .Include(e => e.EmployeeRole)
                .Include(e => e.EmployeeShift)
                .FirstOrDefault(e => e.Id == id);
        }

        public Employee Add(Employee employee)
        {
            if (GetEmployeesByMailAsync(employee.Email))
            {
                throw new Exception("An employee with this email already exists.");
            }
            else dbContext.Employees.Add(employee);

            dbContext.SaveChanges();

            return employee;
        }

        public Employee? Update(Guid id, Employee employee)
        {
            var existingEmployee = dbContext.Employees
                .Include(e => e.EmployeeRole)
                .Include (e => e.EmployeeShift)
                .FirstOrDefault(e => e.Id == id);

            if (existingEmployee == null) return null;

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.EmployeeRoleId = employee.EmployeeRoleId;
            existingEmployee.BossId = employee.BossId;

            existingEmployee.PasswordHash = employee.PasswordHash;

            dbContext.SaveChanges();

            var updatedEmployee = GetById(id);

            return updatedEmployee;
        }

        public bool Delete(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null) return false;
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return true;
        }

        public Role GetRoleById(int id)
        {
            return dbContext.Roles.Find(id);
        }

        public Shift GetShiftById(int id)
        {
            return dbContext.Shifts.Find(id);
        }

        public Guid GetBossIdById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            if (employee.Id == null || employee.Id == Guid.Empty)
            {
                throw new Exception("Boss ID not assigned for this employee.");
            }

            return employee.Id;
        }

        private bool GetEmployeesByMailAsync(string email)
        {
            if (dbContext.Employees
                .Any(e => e.Email.Contains(email)))
            {
                return true;
            }
            return false;
        }

    }
}