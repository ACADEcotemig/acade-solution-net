using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcadeAppApi.Data; // Your DbContext namespace
using AcadeAppApi.Models; // Your Usuario model namespace
using System.Text.Json;

namespace AcadeAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            try
            {
                var list = await _context.Usuarios.ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                // return problem details for debugging (remove in production)
                return Problem(detail: ex.ToString(), title: "Error fetching users");
            }
        }

        // POST: api/usuarios/migrate-passwords
        // Development helper: convert plaintext passwords to bcrypt for all users
        [HttpPost("migrate-passwords")]
        public async Task<IActionResult> MigratePasswords()
        {
            var users = await _context.Usuarios.ToListAsync();
            int updated =0;
            foreach (var u in users)
            {
                var pwd = u.Senha ?? string.Empty;
                if (string.IsNullOrEmpty(pwd)) continue;
                // simple check for bcrypt hash prefix
                if (!(pwd.StartsWith("$2a$") || pwd.StartsWith("$2b$") || pwd.StartsWith("$2y$")))
                {
                    u.Senha = BCrypt.Net.BCrypt.HashPassword(pwd);
                    updated++;
                }
            }
            if (updated >0)
            {
                await _context.SaveChangesAsync();
            }
            return Ok(new { migrated = updated });
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Senha)) return BadRequest("Password required.");

            // hash password
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();

            var existing = await _context.Usuarios.FindAsync(id);
            if (existing == null) return NotFound();

            // update fields explicitly to avoid unintended overwrites
            existing.Nome = usuario.Nome;
            existing.Email = usuario.Email;
            existing.Telefone = usuario.Telefone;
            existing.HistoricoAtv = usuario.HistoricoAtv;
            existing.RelatorioImpacto = usuario.RelatorioImpacto;
            existing.InterfacePref = usuario.InterfacePref;
            existing.IdiomaPref = usuario.IdiomaPref;
            existing.Localizacao = usuario.Localizacao;
            existing.PontosColeta = usuario.PontosColeta;

            // handle password: if client provided a non-empty Senha that's different, hash and set it
            if (!string.IsNullOrEmpty(usuario.Senha) && usuario.Senha != existing.Senha)
            {
                existing.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return Ok(existing);
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return NotFound();

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/usuarios/5/senha
        [HttpPut("{id}/senha")]
        public async Task<IActionResult> UpdateSenha(int id, [FromBody] JsonElement payload)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return NotFound();

            string newPwd;
            try
            {
                newPwd = payload.GetString() ?? string.Empty;
            }
            catch
            {
                newPwd = payload.ToString().Trim('"');
            }

            if (string.IsNullOrEmpty(newPwd)) return BadRequest("Empty password");

            user.Senha = BCrypt.Net.BCrypt.HashPassword(newPwd);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
