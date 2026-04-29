using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PakDzal_Games_API.Data;
using PakDzal_Games_API.Models;

namespace PakDzal_Games_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly GameClubDbContext _context;

    public UsersController(GameClubDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(
        [FromQuery] string? search = null,
        [FromQuery] string? city = null)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(u => u.Name.ToLower().Contains(search) || 
                                     u.Email.ToLower().Contains(search));
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(u => u.City == city);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.UserId) return BadRequest();
        
        _context.Entry(user).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("report")]
    public async Task<ActionResult> GetReport([FromQuery] string? city = null)
    {
        var query = _context.Users.AsQueryable();
        if (!string.IsNullOrEmpty(city))
            query = query.Where(u => u.City == city);
        
        return Ok(new { 
            TotalUsers = await query.CountAsync(),
            City = city ?? "Все",
            ReportDate = DateTime.Now 
        });
    }

    private bool UserExists(int id) => _context.Users.Any(u => u.UserId == id);
}