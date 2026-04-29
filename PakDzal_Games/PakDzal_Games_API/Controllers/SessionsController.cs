using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PakDzal_Games_API.Data;
using PakDzal_Games_API.Models;

namespace PakDzal_Games_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly GameClubDbContext _context;

    public SessionsController(GameClubDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Session>>> GetSessions([FromQuery] int? userId, [FromQuery] int? gameId)
    {
        var query = _context.Sessions.Include(s => s.User).Include(s => s.Game).AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(s => s.UserId == userId.Value);
        }

        if (gameId.HasValue)
        {
            query = query.Where(s => s.GameId == gameId.Value);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Session>> GetSession(int id)
    {
        var session = await _context.Sessions
            .Include(s => s.User)
            .Include(s => s.Game)
            .FirstOrDefaultAsync(s => s.SessionId == id);
        
        if (session == null) return NotFound();
        return session;
    }

    [HttpPost]
    public async Task<ActionResult<Session>> CreateSession(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSession), new { id = session.SessionId }, session);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSession(int id, Session session)
    {
        if (id != session.SessionId) return BadRequest();
        
        _context.Entry(session).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SessionExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSession(int id)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session == null) return NotFound();

        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("report")]
    public async Task<ActionResult> GetReport([FromQuery] int? userId = null, [FromQuery] int? gameId = null)
    {
        var query = _context.Sessions.AsQueryable();
        if (userId.HasValue)
            query = query.Where(s => s.UserId == userId.Value);
        if (gameId.HasValue)
            query = query.Where(s => s.GameId == gameId.Value);
        
        return Ok(new { 
            TotalSessions = await query.CountAsync(),
            TotalRevenue = await query.SumAsync(s => s.TotalPrice ?? 0),
            ReportDate = DateTime.Now 
        });
    }

    private bool SessionExists(int id) => _context.Sessions.Any(s => s.SessionId == id);
}