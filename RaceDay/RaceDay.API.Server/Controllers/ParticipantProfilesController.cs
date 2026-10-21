
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Server.Data;
using RaceDay.Server.Models;

namespace RaceDay.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantProfilesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ParticipantProfilesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/ParticipantProfiles
        [HttpGet]
        public async Task<IActionResult> GetParticipantProfiles()
        {
            var profiles = await _context.ParticipantProfiles
                .AsNoTracking()
                .ToListAsync();

            return Ok(profiles);
        }

        // GET: api/ParticipantProfiles/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetParticipantProfile(int id)
        {
            var profile = await _context.ParticipantProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProfileID == id);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Participant profile not found."
                });
            }

            return Ok(profile);
        }

        // POST: api/ParticipantProfiles
        [HttpPost]
        public async Task<IActionResult> CreateParticipantProfile(
            [FromBody] ParticipantProfile newProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ParticipantProfiles.Add(newProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetParticipantProfile),
                new { id = newProfile.ProfileID },
                newProfile);
        }

        // PUT: api/ParticipantProfiles/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateParticipantProfile(
            int id,
            [FromBody] ParticipantProfile updatedProfile)
        {
            if (id != updatedProfile.ProfileID)
            {
                return BadRequest(new
                {
                    message = "The profile ID in the URL must match the ID in the request body."
                });
            }

            var existingProfile = await _context.ParticipantProfiles
                .FirstOrDefaultAsync(p => p.ProfileID == id);

            if (existingProfile == null)
            {
                return NotFound(new
                {
                    message = "Participant profile not found."
                });
            }

            _context.Entry(existingProfile)
                .CurrentValues.SetValues(updatedProfile);

            await _context.SaveChangesAsync();

            return Ok(existingProfile);
        }

        // DELETE: api/ParticipantProfiles/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteParticipantProfile(int id)
        {
            var profile = await _context.ParticipantProfiles
                .FirstOrDefaultAsync(p => p.ProfileID == id);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Participant profile not found."
                });
            }

            _context.ParticipantProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Participant profile deleted successfully."
            });
        }
    }
}
