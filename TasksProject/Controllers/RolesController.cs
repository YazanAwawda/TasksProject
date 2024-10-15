using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TasksProject.DataEntity;
using TasksProject.Dto;
using TasksProject.Models;

namespace TasksProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly TasksDbContext _context;
        private readonly IMapper _mapper;

        public RolesController(TasksDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Roles>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        [HttpPost]
        public async Task<ActionResult<Roles>> PostRole(RoleDto roleDto)
        {

            // Step 1: Validate the incoming RoleDto
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return 400 Bad Request if validation fails
            }

            // Step 2: Use AutoMapper to map RoleDto to Role entity
            var role = _mapper.Map<Roles>(roleDto);

            // Step 3: Add the new role to the database
            _context.Roles.Add(role);

            // Step 4: Save changes to the database
            await _context.SaveChangesAsync();

            // Step 5: Return a response with the created role's location
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleDto roleDto)
        {

            // Step 1: Validate the ID
            if (id != roleDto.Id)
            {
                return BadRequest("Role ID mismatch");
            }

            // Step 2: Fetch the existing role from the database
            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null)
            {
                return NotFound($"Role with ID {id} not found");
            }

            // Step 3: Use AutoMapper to update the existing role with data from RoleDto
            _mapper.Map(roleDto, existingRole);  // Map updates from roleDto to existingRole

            try
            {
                // Step 4: Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))  // Check if the role still exists in case of concurrency issues
                {
                    return NotFound($"Role with ID {id} not found after update attempt.");
                }
                else
                {
                    throw;  // If it's some other concurrency exception, rethrow
                }
            }

            // Step 5: Return NoContent (204) to indicate successful update
            return NoContent();
            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}


