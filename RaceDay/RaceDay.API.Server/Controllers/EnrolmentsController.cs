using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Server.Data;
using RaceDay.Server.Models;

namespace RaceDay.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EnrolmentsController(RaceDayDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetEnrolments()
    {
        var query = _context.Enrolments.AsQueryable();

        if (User.IsInRole("Participant"))
        {
            query = query.Where(e => e.ParticipantID == CurrentUserId);
        }
        else if (User.IsInRole("Organiser"))
        {
            query = query.Where(e => _context.Events.Any(ev =>
                ev.EventID == e.EventID &&
                ev.OrganiserID == CurrentUserId));
        }

        var enrolments = await query
            .OrderByDescending(e => e.EnrolmentDate)
            .ToListAsync();

        return Ok(enrolments);
    }

    [Authorize(Roles = "Participant")]
    [HttpPost]
    public async Task<IActionResult> CreateEnrolment(
        CreateEnrolmentRequest request)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryID == request.CategoryID);

        if (category == null || category.EventID != request.EventID)
        {
            return BadRequest(new
            {
                message = "The selected category does not belong to this event."
            });
        }

        var eventExists = await _context.Events
            .AnyAsync(e => e.EventID == request.EventID);

        if (!eventExists)
            return NotFound(new { message = "Event not found." });

        var alreadyEnrolled = await _context.Enrolments.AnyAsync(e =>
            e.ParticipantID == CurrentUserId &&
            e.EventID == request.EventID);

        if (alreadyEnrolled)
        {
            return Conflict(new
            {
                message = "You are already enrolled in this event."
            });
        }

        var enrolment = new Enrolment
        {
            ParticipantID = CurrentUserId,
            EventID = request.EventID,
            CategoryID = request.CategoryID,
            EnrolmentDate = DateTime.UtcNow,
            EnrolmentStatus = "Active"
        };

        _context.Enrolments.Add(enrolment);
        await _context.SaveChangesAsync();

        return Created(
            $"/api/Enrolments/{enrolment.EnrolmentID}",
            enrolment);
    }

    [Authorize(Roles = "Organiser")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnrolment(
        int id, UpdateEnrolmentRequest request)
    {
        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.EnrolmentID == id);

        if (enrolment == null)
            return NotFound();

        var belongsToOrganiser = await _context.Events.AnyAsync(e =>
            e.EventID == enrolment.EventID &&
            e.OrganiserID == CurrentUserId);

        if (!belongsToOrganiser)
            return Forbid();

        enrolment.EnrolmentStatus = request.EnrolmentStatus;
        await _context.SaveChangesAsync();

        return Ok(enrolment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnrolment(int id)
    {
        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.EnrolmentID == id);

        if (enrolment == null)
            return NotFound();

        var isOwner = User.IsInRole("Participant") &&
                      enrolment.ParticipantID == CurrentUserId;

        var isOrganiserOfEvent = User.IsInRole("Organiser") &&
            await _context.Events.AnyAsync(e =>
                e.EventID == enrolment.EventID &&
                e.OrganiserID == CurrentUserId);

        if (!isOwner && !isOrganiserOfEvent)
            return Forbid();

        _context.Enrolments.Remove(enrolment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class CreateEnrolmentRequest
{
    public int EventID { get; set; }
    public int CategoryID { get; set; }
}

public class UpdateEnrolmentRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.RegularExpression(
        "^(Active|Pending|Confirmed|Cancelled)$")]
    public string EnrolmentStatus { get; set; } = "Active";
}