using FullStackAssessment_U3Infotech.Application.Commands.CafeCommands;
using FullStackAssessment_U3Infotech.Application.Queries.CafeQuery;
using FullStackAssessment_U3Infotech.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStackAssessment_U3Infotech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CafeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetCafes(string? location)
        {
            var query = new GetCafesQuery { Location = location };
            var cafes = await _mediator.Send(query);
            return Ok(cafes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCafe(CreateCafeCmd command)
        {
            var cafe = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCafes), new { id = cafe.ID }, cafe);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCafe(int id, UpdatedCafe updatedCafe)
        {
            if (id != updatedCafe.Id)
            {
                return BadRequest("Cafe ID does not match");
            }

            var command = new UpdatedCafe(updatedCafe.Id, updatedCafe.Name, updatedCafe.Location, updatedCafe.Description);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"Cafe with ID {id} not found.");
            }

            return NoContent();

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCafe( int id)
        {
            var command = new DeleteCafeCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"Cafe with ID {id} not found.");
            }

            return NoContent();

        }
    }
}
