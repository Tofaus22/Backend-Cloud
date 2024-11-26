using Microsoft.AspNetCore.Mvc;
using WebApiCloud3.Context;
using WebApiCloud3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UsuarioController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok(usuario);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Usuario login)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == login.Correo && u.Contrasena == login.Contrasena);

        if (usuario == null)
            return Unauthorized();

        var token = GenerateJwtToken(usuario);
        usuario.Token = token;
        await _context.SaveChangesAsync();

        return Ok(new { Token = token });
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
    {
        // Verifica si el id del usuario en la URL coincide con el id del cuerpo del usuario
        if (id != usuario.Id_Usuario)
        {
            return BadRequest("El ID del usuario no coincide.");
        }

        // Verifica si el usuario existe en la base de datos
        var usuarioExistente = await _context.Usuarios.FindAsync(id);
        if (usuarioExistente == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        // Actualiza los valores del usuario
        usuarioExistente.Nombre = usuario.Nombre;
        usuarioExistente.Apellido = usuario.Apellido;
        usuarioExistente.Correo = usuario.Correo;
        usuarioExistente.Contrasena = usuario.Contrasena; // Si es necesario, agrega más campos

        // Guarda los cambios en la base de datos
        await _context.SaveChangesAsync();

        return NoContent(); // Responde con 204 No Content
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        // Busca el usuario por su id
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        // Elimina el usuario de la base de datos
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent(); // Responde con 204 No Content
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Correo),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
