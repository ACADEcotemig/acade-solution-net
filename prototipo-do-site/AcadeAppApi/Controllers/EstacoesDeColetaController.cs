using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcadeAppApi.Data;
using AcadeAppApi.Models;

namespace AcadeAppApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstacoesDeColetaController : ControllerBase
{
    private readonly AppDbContext _context;

    public EstacoesDeColetaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstacaoDeColeta>>> GetEstacoes()
        => await _context.EstacoesDeColeta.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<EstacaoDeColeta>> GetEstacao(int id)
    {
        var estacao = await _context.EstacoesDeColeta.FindAsync(id);
        if (estacao == null) return NotFound();
        return estacao;
    }

    [HttpPost]
    public async Task<ActionResult<EstacaoDeColeta>> PostEstacao(EstacaoDeColeta estacao)
    {
        _context.EstacoesDeColeta.Add(estacao);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEstacao), new { id = estacao.Id }, estacao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEstacao(int id, EstacaoDeColeta estacao)
    {
        if (id != estacao.Id) return BadRequest();
        _context.Entry(estacao).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEstacao(int id)
    {
        var estacao = await _context.EstacoesDeColeta.FindAsync(id);
        if (estacao == null) return NotFound();
        _context.EstacoesDeColeta.Remove(estacao);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
