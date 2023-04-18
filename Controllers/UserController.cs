using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chat.Models;
using Microsoft.AspNetCore.SignalR;
using Chat.Hubs;

namespace Chat.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
      private readonly DatabaseContext _context;
    private readonly IHubContext<ChatHub> _hub;
    public UsersController(DatabaseContext context, IHubContext<ChatHub> hub)
    {
        _context = context;
        _hub = hub;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var User = await _context.Users.FindAsync(id);

        if (User == null)
        {
            return NotFound();
        }

        return User;
    }

       [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Send a message to all clients listening to the hub
        await _hub.Clients.All.SendAsync("ReceiveMessage", user);

        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, User User)
    {
        if (id != User.Id)
        {
            return BadRequest();
        }

        _context.Entry(User).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var User = await _context.Users.FindAsync(id);
        if (User == null)
        {
            return NotFound();
        }

        _context.Users.Remove(User);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}