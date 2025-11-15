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


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var task = await _context.TaskInputs.FindAsync(Id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
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

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateStatus(int Id, [FromBody] UpdateStatusDto updateStatusDto)
        {
            var updateTask = await _context.TaskInputs.FindAsync(Id);
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

            return Ok(new UpdateStatusOutputDto { 
                Id = updateTask.Id,
                Status =  updateTask.Status
            });
        }

        [HttpPut("{Id}/description")]

        public async Task<IActionResult> UpdateDescription(int Id, [FromBody] DtoEditDescription editDescriptionDto)
        {
            var editDescription = await _context.TaskInputs.FindAsync(Id);
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
                Id = editDescription.Id,
                Description = editDescription.Description
            });
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var deleteTask = await _context.TaskInputs.FindAsync(Id);
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
