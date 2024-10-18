using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksProject.DataEntity;
using TasksProject.Dto;
using TasksProject.Models;

namespace TasksProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TasksDbContext _context;
        private readonly IMapper _mapper;

        public TasksController(TasksDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpPost]
        public async Task<ActionResult<Tasks>> PostTask(TaskDto taskDto)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var task = _mapper.Map<Tasks>(taskDto);

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskDto taskDto)
        {

            if (id != taskDto.Id)
            {
                return BadRequest("Task ID mismatch");
            }

            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound($"Task with ID {id} not found");
            }

            _mapper.Map(taskDto, existingTask);  

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))  
                {
                    return NotFound($"Task with ID {id} not found after update attempt.");
                }
                else
                {
                    throw;  
                }
            }

            return NoContent();


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }

        [HttpPatch("assign")]
        public async Task<IActionResult> AssignTask(AssignTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(dto.TaskId);
            if (task == null)
                return NotFound("Task not found");

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found");

            task.UserId = dto.UserId;
            task.TaskStatus = Status.Assigned;

            await _context.SaveChangesAsync();
            return Ok("Task assigned successfully");
        }

        [HttpPatch("start")]
        public async Task<IActionResult> StartTask(ChangeTaskStatusDto dto)
        {
            var task = await _context.Tasks.FindAsync(dto.Id);
            if (task == null)
                return NotFound("Task not found");

            task.Id  = dto.Id;
            task.TaskStatus = Status.Started;
            
            await _context.SaveChangesAsync();
            return Ok("Task started successfully");
        }

        [HttpPatch("close")]
        public async Task<IActionResult> CloseTask(ChangeTaskStatusDto dto)
        {
            var task = await _context.Tasks.FindAsync(dto.Id);
            if (task == null)
                return NotFound("Task not found");
            // Use AutoMapper to map from the DTO to the existing task entity
            _mapper.Map(dto, task);
            task.Id = dto.Id;
            task.TaskStatus = Status.Closed;

            await _context.SaveChangesAsync();
            return Ok("Task closed successfully");
        }

        [HttpPatch("resolve")]
        public async Task<IActionResult> ResolveTask(ChangeTaskStatusDto dto)
        {
            var task = await _context.Tasks.FindAsync(dto.Id);
            if (task == null)
                return NotFound("Task not found");

            task.Id = dto.Id;
            task.TaskStatus = Status.Resolved;

            await _context.SaveChangesAsync();
            return Ok("Task resolved successfully");
        }



        [HttpPatch("inprogress")]
        public async Task<IActionResult> TaskInProgress(ChangeTaskStatusDto dto)
        {
            var task = await _context.Tasks.FindAsync(dto.Id);
            if (task == null)
                return NotFound("Task not found");

            task.Id = dto.Id;
            task.TaskStatus = Status.InProgress;

            await _context.SaveChangesAsync();
            return Ok("Task InProgress ");
        }


        [HttpGet("GetTasksByUserID/{usreId}")]
        public async Task<ActionResult<Tasks>> GetTasksByUserID(int userId)
        {
            var user = await _context.Tasks
                .Where(u => u.UserId == userId).ToListAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Return the  user tasks details
            return Ok(user);
        }

    }
}
