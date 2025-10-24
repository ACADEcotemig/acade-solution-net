using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcadeAppApi.Data;
using AcadeAppApi.Models;
namespace AcadeAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrabalhadoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public TrabalhadoresController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trabalhador>>> GetTrabalhadores()
        => await _context.Trabalhadores.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Trabalhador>> GetTrabalhador(int id)
    {
        var trabalhador = await _context.Trabalhadores.FindAsync(id);
        if (trabalhador == null) return NotFound();
        return trabalhador;
    }

    [HttpPost]
    public async Task<ActionResult<Trabalhador>> PostTrabalhador(Trabalhador trabalhador)
    {
        _context.Trabalhadores.Add(trabalhador);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTrabalhador), new { id = trabalhador.Id }, trabalhador);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTrabalhador(int id, Trabalhador trabalhador)
    {
        if (id != trabalhador.Id) return BadRequest();
        _context.Entry(trabalhador).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrabalhador(int id)
    {
        var trabalhador = await _context.Trabalhadores.FindAsync(id);
        if (trabalhador == null) return NotFound();
        _context.Trabalhadores.Remove(trabalhador);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
