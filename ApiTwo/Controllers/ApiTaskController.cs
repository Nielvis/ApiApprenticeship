using ApiTwo.Implementations;
using Microsoft.EntityFrameworkCore;
using ApiTwo.Models;
using Microsoft.AspNetCore.Mvc;
using ApiTwo.Models.DTO.Input;
using ApiTwo.Models.DTO.Output;

namespace ApiTwo.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ApiTaskController : ControllerBase
    {
        private readonly ILogger<ApiTaskController> _logger;
        private readonly ApiDbContext _context;

        public ApiTaskController(ILogger<ApiTaskController> logger,
                ApiDbContext context
            )
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var tasks = await _context.TaskInputs.ToListAsync();
            return Ok(tasks);
        }


        [HttpGet("{taskId}/clients")]
        
        public async Task<IActionResult> GetClientsForTask(int taskId, [FromServices] ApiDbContext db, [FromServices] IHttpClientFactory httpClientFactory)
        {
            var clientsId = db.TaskClients.Where(c => c.TaskId == taskId).Select(c => c.ClientId).ToList();

            var http = httpClientFactory.CreateClient("ApiOne");

            var clients = new List<object>();

            foreach (var id in clientsId) 
            {
                var resp = await http.GetAsync($"/clients/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    var c = await resp.Content.ReadFromJsonAsync<object>();
                    clients.Add(c);
                }
            }
            return Ok(clients);
        }

        [HttpPost("{taskId}/assign-client/{clientId}")]
        
        public async Task<IActionResult> AssignClientForTask(int taskId, int clientId, [FromServices] ApiDbContext db, [FromServices] IHttpClientFactory httpClientFactory)
        {
            var task = await _context.TaskInputs.FindAsync(taskId);
            if (task == null)
            {
                NotFound("Tarefa não encontrada");
            }

            var http = httpClientFactory.CreateClient("ApiOne");
            var resp = await http.GetAsync($"/clients/{clientId}");
            if (!resp.IsSuccessStatusCode) 
            {
                NotFound("Cliente não existe.");
            }

            var linkExists = db.TaskClients.Any(c => c.TaskId == taskId && c.ClientId == clientId);
            if (linkExists) 
            {
                return BadRequest("O cliente já está vinculado a uma tarefa!");
            }

            db.TaskClients.Add(new TaskClient
            {
                TaskId = taskId,
                ClientId = clientId,
            });
            await db.SaveChangesAsync();

            return Ok("Cliente vinculado à tarefa.");
        }

        [HttpGet("{TaskId}")]
        public async Task<IActionResult> GetById(int taskId)
        {
            var task = await _context.TaskInputs.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpGet("{clientId}/tasks")]
        public async Task<IActionResult> GetTasksForClient(int clientId)
        {
            var tasks = await _context.TaskClients
                .Where(c => c.ClientId == clientId).Select(c => c.Task).ToListAsync();

            if (!tasks.Any())
                return NotFound("Sem tarefas encontradas para esse cliente.");

            return Ok(tasks);
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] TaskInput postTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.TaskInputs.AddAsync(postTask);
            await _context.SaveChangesAsync();
            return Ok(postTask);
        }

        [HttpPut("{TaskId}")]
        public async Task<IActionResult> UpdateStatus(int taskId, [FromBody] UpdateStatusDto updateStatusDto)
        {
            var updateTask = await _context.TaskInputs.FindAsync(taskId);
            if (updateTask == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            if (updateStatusDto.Status == null)
            {
                return BadRequest("Status não pode ser nulo");
            }

            updateTask.Status = updateStatusDto.Status;
            await _context.SaveChangesAsync();

            return Ok(new UpdateStatusOutputDto
            {
                TaskId = updateTask.TaskId,
                Status = updateTask.Status
            });
        }

        [HttpPut("{TaskId}/description")]
        public async Task<IActionResult> UpdateDescription(int taskId, [FromBody] DtoEditDescription editDescriptionDto)
        {
            var editDescription = await _context.TaskInputs.FindAsync(taskId);
            if (editDescription == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            if (editDescriptionDto.Description == null)
            {
                return BadRequest("Descrição não pode ser nula");
            }

            editDescription.Description = editDescriptionDto.Description;
            await _context.SaveChangesAsync();

            return Ok(new EditDescriptionOutputDto
            {
                TaskId = editDescription.TaskId,
                Description = editDescription.Description
            });
        }

        [HttpDelete("{TaskId}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            var deleteTask = await _context.TaskInputs.FindAsync(taskId);
            if (deleteTask == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            _context.TaskInputs.Remove(deleteTask);
            await _context.SaveChangesAsync();
            return Ok("Tarefa deletada com sucesso");
        }
    }
}
