using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ApiRestNetNoxun.Data;
using ApiRestNetNoxun.Dtos;
using ApiRestNetNoxun.Models;
using ApiRestNetNoxun.Helpers;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace ApiRestNetNoxun.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
        return BadRequest(new { message = "El nombre de usuario ya existe" });

    // Buscar el rol en la base
    var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == dto.RoleName);
    if (role == null)
        return BadRequest(new { message = $"El rol '{dto.RoleName}' no existe." });

    var user = new User
    {
        Username = dto.Username,
        Email = dto.Email
    };

    var passwordHasher = new PasswordHasher<User>();
    user.Password = passwordHasher.HashPassword(user, dto.Password);

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Asignar el rol encontrado
    _context.UserRoles.Add(new UserRole { UserID = user.UserID, RoleID = role.RoleID });
    await _context.SaveChangesAsync();

    return Ok(new
    {
        message = "Usuario registrado correctamente",
        user = new
        {
            id = user.UserID,
            username = user.Username,
            email = user.Email,
            role = role.RoleName
        }
    });
}
/*
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var roles = await _context.UserRoles
                .Where(ur => ur.UserID == user.UserID)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();

            var token = JwtHelper.GenerateToken(user, roles, _config);

            return Ok(new
            {
                message = "Login exitoso",
                token = token,
                user = new
                {
                    id = user.UserID,
                    username = user.Username
                },
                roles = roles
            });
        }

*/
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
    if (user == null)
        return Unauthorized(new { message = "Credenciales inválidas" });

    var passwordHasher = new PasswordHasher<User>();
    var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
    if (result == PasswordVerificationResult.Failed)
        return Unauthorized(new { message = "Credenciales inválidas" });

    var roles = await _context.UserRoles
        .Where(ur => ur.UserID == user.UserID)
        .Select(ur => ur.Role.RoleName)
        .ToListAsync();

    var token = JwtHelper.GenerateToken(user, roles, _config);

    // Cargar toda la información relacionada para la respuesta
    var users = await _context.Users
        .Select(u => new {
            u.UserID,
            u.Username,
            Email = u.Email
        }).ToListAsync();

    var allRoles = await _context.Roles
        .Select(r => new {
            r.RoleID,
            r.RoleName,
            r.Description
        }).ToListAsync();

    var userRoles = await _context.UserRoles
        .Select(ur => new {
            ur.ID,
            ur.UserID,
            ur.RoleID
        }).ToListAsync();

    var procedures = await _context.Procedures
        .Select(p => new {
            p.ProcedureID,
            p.ProcedureName,
            p.Description,
            p.CreatedByUserID,
            p.LastModifiedUserID
        }).ToListAsync();

    var fields = await _context.Fields
        .Select(f => new {
            f.FieldID,
            f.FieldName,
            f.DataType
        }).ToListAsync();

    var dataSets = await _context.DataSets
        .Select(d => new {
            d.DataSetID,
            d.DataSetName,
            d.Description,
            ProcedureID = d.ProcedureID,
            FieldID = d.FieldID
        }).ToListAsync();

    return Ok(new
    {
        message = "Login exitoso",
        token = token,
        user = new
        {
            id = user.UserID,
            username = user.Username
        },
        userRoles = roles,      // lista de roles del usuario que hizo login
        usersList = users,      // lista general de usuarios
        rolesList = allRoles,   // lista general de roles
        userRolesList = userRoles, // lista general de relaciones UserRoles
        proceduresList = procedures,
        fieldsList = fields,
        dataSetsList = dataSets
    });
}




        
    }
}
