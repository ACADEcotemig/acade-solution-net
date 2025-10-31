using Microsoft.AspNetCore.Mvc;
using AcadeAppApi.Data;
using AcadeAppApi.Models;
using AcadeAppApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AcadeAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
 private readonly AppDbContext _context;
 private readonly ITokenService _tokenService;

 public AuthController(AppDbContext context, ITokenService tokenService)
 {
 _context = context;
 _tokenService = tokenService;
 }

 // POST: api/auth/login
 [HttpPost("login")]
 public async Task<IActionResult> Login([FromBody] LoginRequest request)
 {
 if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrEmpty(request.Senha))
 return BadRequest("Email and password required.");

 var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
 if (user == null) return Unauthorized("Invalid credentials.");

 bool valid = false;

 try
 {
 // If stored password looks like a bcrypt hash, verify normally
 if (!string.IsNullOrEmpty(user.Senha) && (user.Senha.StartsWith("$2a$") || user.Senha.StartsWith("$2b$") || user.Senha.StartsWith("$2y$")))
 {
 valid = BCrypt.Net.BCrypt.Verify(request.Senha, user.Senha);
 }
 else
 {
 // legacy/plaintext password stored — check direct match, then migrate to bcrypt
 if (user.Senha == request.Senha)
 {
 valid = true;
 // Hash and update stored password for future security
 user.Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha);
 _context.Usuarios.Update(user);
 await _context.SaveChangesAsync();
 }
 }
 }
 catch
 {
 // If bcrypt Verify throws for some reason treat as invalid
 valid = false;
 }

 if (!valid) return Unauthorized("Invalid credentials.");

 var token = _tokenService.CreateToken(user);

 return Ok(new { token, user = new { user.Id, user.Nome, user.Email } });
 }
}
