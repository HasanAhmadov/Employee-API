using Microsoft.AspNetCore.Mvc;
using MicroServices.BusinessLayer.DTOs;
using MicroServices.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace APIPractice.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        { 
            return Ok(employeeService.GetAll()); 
        }

        [HttpGet("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = employeeService.GetById(id);
            if (employee == null) return NotFound();
            else return Ok(employee);
        }

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee(EmployeeDTO employeeDTO)
        {
            var employeeEntity = employeeService.Add(employeeDTO);
            return Ok(employeeEntity);
        }

        [HttpPut("UpdateEmployee/{id}")]
        public IActionResult UpdateEmployee(Guid id, EmployeeDTO employeeDTO)
        {
            var employee = employeeService.Update(id, employeeDTO);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpDelete("DeleteEmployee")]
        public IActionResult DeleteEmployee(Guid id) {

            var employee = employeeService.Delete(id);   

            return Ok(employee);
        
        }
    }
}