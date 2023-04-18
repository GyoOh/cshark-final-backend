using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chat.Models;
using Microsoft.AspNetCore.SignalR;
using Chat.Hubs;

namespace Chat.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelsController : ControllerBase
{
      private readonly DatabaseContext _context;
    private readonly IHubContext<ChatHub> _hub;
    public ChannelsController(DatabaseContext context, IHubContext<ChatHub> hub)
    {
        _context = context;
        _hub = hub;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Channel>>> GetChannels()
    {
        return await _context.Channels.ToListAsync();

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Channel>> GetChannel(Guid id)
    {
        var Channel = await _context.Channels.FindAsync(id);

        if (Channel == null)
        {
            return NotFound();
        }

        return Channel;
    }

          [HttpPost("uid/{creatorId}")]
    public async Task<ActionResult<Channel>> PostChannel(Channel channel)
    {
        _context.Channels.Add(channel);
        await _context.SaveChangesAsync();

        // Send a message to all clients listening to the hub
        await _hub.Clients.All.SendAsync("ReceiveMessage", channel);

        return CreatedAtAction("GetChannel", new { id = channel.Id }, channel);
    }


     [HttpPost("{channelId}/Messages")]
    public async Task<Message> PostChannelMessage(string channelId, Message Message)
    {
        Message.ChannelId = channelId;
        _context.Messages.Add(Message);
        await _context.SaveChangesAsync();

        await _hub.Clients.Group(channelId.ToString()).SendAsync("ReceiveMessage", Message);

        return Message;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutChannel(Guid id, Channel Channel)
    {
        if (id != Channel.Id)
        {
            return BadRequest();
        }

        _context.Entry(Channel).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetChannel", new { id = Channel.Id }, Channel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChannel(Guid id)
    {
        var Channel = await _context.Channels.FindAsync(id);
        if (Channel == null)
        {
            return NotFound();
        }

        _context.Channels.Remove(Channel);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}