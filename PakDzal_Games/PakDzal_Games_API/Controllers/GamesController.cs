using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PakDzal_Games_API.Data;
using PakDzal_Games_API.Models;

namespace PakDzal_Games_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameClubDbContext _context;

    public GamesController(GameClubDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetGames(
        [FromQuery] string? search = null,
        [FromQuery] string? genre = null,
        [FromQuery] bool? available = null)
    {
        var query = _context.Games.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(g => g.Title.ToLower().Contains(search) || 
                                  g.Genre.ToLower().Contains(search));
        }

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(g => g.Genre == genre);
        }

        if (available.HasValue)
        {
            query = query.Where(g => g.Available == available.Value);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null) return NotFound();
        return game;
    }

    [HttpPost]
    public async Task<ActionResult<Game>> CreateGame(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGame(int id, Game game)
    {
        if (id != game.GameId) return BadRequest();
        
        _context.Entry(game).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GameExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null) return NotFound();

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("report")]
    public async Task<ActionResult> GetReport([FromQuery] bool? available = null)
    {
        var query = _context.Games.AsQueryable();
        if (available.HasValue)
            query = query.Where(g => g.Available == available.Value);
        
        return Ok(new { 
            TotalGames = await query.CountAsync(),
            AvailableGames = await query.CountAsync(g => g.Available),
            ReportDate = DateTime.Now 
        });
    }

    private bool GameExists(int id) => _context.Games.Any(g => g.GameId == id);
}