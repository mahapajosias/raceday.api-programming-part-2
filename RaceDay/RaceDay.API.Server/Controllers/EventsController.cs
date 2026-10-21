
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Server.Data;
using RaceDay.Server.Models;

namespace RaceDay.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .AsNoTracking()
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/Events/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var raceEvent = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
            {
                return NotFound(new
                {
                    message = "Race event not found."
                });
            }

            return Ok(raceEvent);
        }

        // POST: api/Events
        [HttpPost]
        public async Task<IActionResult> CreateEvent(
            [FromBody] Event newEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = newEvent.EventID },
                newEvent);
        }

        // PUT: api/Events/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            [FromBody] Event updatedEvent)
        {
            if (id != updatedEvent.EventID)
            {
                return BadRequest(new
                {
                    message = "The event ID in the URL must match the event ID in the request body."
                });
            }

            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (existingEvent == null)
            {
                return NotFound(new
                {
                    message = "Race event not found."
                });
            }

            existingEvent.OrganiserID = updatedEvent.OrganiserID;
            existingEvent.EventName = updatedEvent.EventName;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Distance = updatedEvent.Distance;
            existingEvent.EventType = updatedEvent.EventType;

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }
    
    
        // DELETE: api/Events/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var raceEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
            {
                return NotFound(new
                {
                    message = "Race event not found."
                });
            }

            _context.Events.Remove(raceEvent);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Race event deleted successfully."
            });
        }

    }
}
