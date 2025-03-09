using FullStackAssessment_U3Infotech.Application.Commands.EmployeeCommands;
using FullStackAssessment_U3Infotech.Application.Queries.EmployeeQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FullStackAssessment_U3Infotech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees(int? cafeID)
        {
            var query = new GetEmployeesQuery { CafeId = cafeID };
            var cafes = await _mediator.Send(query);
            return Ok(cafes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid employee data.");
            }

            var employeeRes = await _mediator.Send(request);
            return CreatedAtAction(nameof(CreateEmployee), new { id = employeeRes.ID }, employeeRes);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(string employeeID,UpdatedEmployee updatedEmployee)
        {
            if (string.IsNullOrEmpty(employeeID))
            {
                return BadRequest("Invalid employee data.");
            }

            var command = new UpdatedEmployee(updatedEmployee.EmployeeID, updatedEmployee.Name, updatedEmployee.Email_Address, updatedEmployee.Phonenumber, updatedEmployee.Gender, updatedEmployee.CafeID);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"Employee with ID {employeeID} not found.");
            }

            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var command = new DeleteEmployeeCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return NoContent();

        }
    }
}
