using ApiOne.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiOne.Implementations;
using Microsoft.EntityFrameworkCore;

namespace ApiOne.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]

    public class ApiOneController : ControllerBase
    {
        private readonly ILogger<ApiOneController> _logger;
        private readonly ApiDbContext _context;
        public ApiOneController(ILogger<ApiOneController> logger,
                ApiDbContext context
            )
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.ClientInputs.ToListAsync());
        }

        [HttpGet("{Id}")]

        public async Task<IActionResult> Get(int Id)
        {
            var client = await _context.ClientInputs.FindAsync(Id);

            if (client == null) 
            {
                return NotFound($"Client not at {Id} Found");
            }

            return Ok(client);
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] ClientInput postClient)
        {
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            await _context.ClientInputs.AddAsync(postClient);
            await _context.SaveChangesAsync();

            return Ok(postClient);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var deleteClient = await _context.ClientInputs.FindAsync(Id);
            if (deleteClient == null)
            {
                return NotFound("Client not exist");
            }
            _context.ClientInputs.Remove(deleteClient);
            await _context.SaveChangesAsync();

            return Ok(deleteClient);
        }
    }
}
