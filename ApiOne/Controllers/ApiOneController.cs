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


        [HttpGet("{clientId}/tasks")]
        public async Task<IActionResult> GetTasksForClients(int clientId, [FromServices] IHttpClientFactory httpClientFactory)
        {
            var client = httpClientFactory.CreateClient("ApiTask");
            var response = await client.GetAsync($"/v1/ApiTask/{clientId}/tasks");

            if(!response.IsSuccessStatusCode)
            {
                return NotFound("Tasks not found for the client");
            }

            var tasks = await response.Content.ReadFromJsonAsync<List<object>>();
            return Ok(tasks);
        }

        [HttpGet("{clientId}")]

        public async Task<IActionResult> Get(int clientId)
        {
            var client = await _context.ClientInputs.FindAsync(clientId);

            if (client == null) 
            {
                return NotFound($"Client not at {clientId} Found");
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

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            var deleteClient = await _context.ClientInputs.FindAsync(clientId);
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
