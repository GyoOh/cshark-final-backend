using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Hubs;
using Chat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        private readonly IHubContext<ChatHub> _hub;

        public MessagesController(
            DatabaseContext context,
            IHubContext<ChatHub> hub
        )
        {
            _context = context;
            _hub = hub;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            if (_context.Messages == null)
            {
                return NotFound();
            }
            return await _context
                .Messages
                .OrderBy(m => m.Created)
                .ToListAsync();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(Guid id)
        {
            if (_context.Messages == null)
            {
                return NotFound();
            }
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(Guid id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            if (_context.Messages == null)
            {
                return Problem("Entity set 'DatabaseContext.Messages'  is null.");
            }
            _context.Messages.Add (message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage",
            new { id = message.Id },
            message);
        }

        [HttpPost("cid/{channelId}/uid/{UserId}/")]
        public async Task<Message>
        PostUserMessage(string channelId, string UserId, Message Message)
        {
        
          YelpAPI yelp = new YelpAPI();
    var businesses = await yelp.SearchBusinessesAsync(Message.Text);

    Random rand = new Random();
    int randomIndex = rand.Next(businesses.Count);
    string randomBusiness = businesses[randomIndex].Name;


        Message.Text = randomBusiness;

            Message.ChannelId = channelId;
            Message.UserId = UserId;
            _context.Messages.Add (Message);
            await _context.SaveChangesAsync();

            await _hub
                .Clients
                .Group(channelId.ToString())
                .SendAsync("ReceiveMessage", Message);
            return Message;
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            if (_context.Messages == null)
            {
                return NotFound();
            }
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove (message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(Guid id)
        {
            return (_context.Messages?.Any(e => e.Id == id))
                .GetValueOrDefault();
        }
    }
}
