using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRestNetNoxun.Data;
using ApiRestNetNoxun.Models;
using ApiRestNetNoxun.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRestNetNoxun.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();

            var userDtos = users.Select(u => new UserDto
            {
                UserID = u.UserID,
                Username = u.Username,
                Email = u.Email,
                Roles = u.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            }).ToList();

            return Ok(new { message = "Usuarios obtenidos correctamente", Users = userDtos });
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado." });

            var userDto = new UserDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            };

            return Ok(new { message = "Usuario obtenido correctamente", User = userDto });
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDto)
        {
            if (id != userDto.UserID)
                return BadRequest(new { message = "El ID del usuario no coincide con el parámetro." });

            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado." });

            // Actualizar campos básicos
            user.Username = userDto.Username;
            user.Email = userDto.Email;

            // Actualizar roles
            _context.UserRoles.RemoveRange(user.UserRoles);

            if (userDto.Roles != null && userDto.Roles.Count > 0)
            {
                foreach (var roleName in userDto.Roles)
                {
                    var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
                    if (role != null)
                    {
                        _context.UserRoles.Add(new UserRole { UserID = user.UserID, RoleID = role.RoleID });
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Usuario con ID {id} actualizado correctamente." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.UserID == id))
                    return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
                else
                    throw;
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado." });

            var userRoles = _context.UserRoles.Where(ur => ur.UserID == id);
            _context.UserRoles.RemoveRange(userRoles);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Usuario con ID {id} eliminado correctamente." });
        }
    }
}
