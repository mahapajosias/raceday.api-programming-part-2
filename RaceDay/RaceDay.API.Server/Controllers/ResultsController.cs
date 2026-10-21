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
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetResults()
    {
        var query = _context.Results.AsQueryable();

        if (User.IsInRole("Participant"))
        {
            query = query.Where(r => _context.Enrolments.Any(e =>
                e.EnrolmentID == r.EnrolmentID &&
                e.ParticipantID == CurrentUserId));
        }
        else if (User.IsInRole("Organiser"))
        {
            query = query.Where(r => _context.Enrolments.Any(e =>
                e.EnrolmentID == r.EnrolmentID &&
                _context.Events.Any(ev =>
                    ev.EventID == e.EventID &&
                    ev.OrganiserID == CurrentUserId)));
        }

        return Ok(await query.ToListAsync());
    }

    [Authorize(Roles = "Organiser")]
    [HttpPost]
    public async Task<IActionResult> CreateResult(ResultRequest request)
    {
        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.EnrolmentID == request.EnrolmentID);

        if (enrolment == null)
            return NotFound(new { message = "Enrolment not found." });

        var ownsEvent = await _context.Events.AnyAsync(e =>
            e.EventID == enrolment.EventID &&
            e.OrganiserID == CurrentUserId);

        if (!ownsEvent)
            return Forbid();

        if (await _context.Results.AnyAsync(
            r => r.EnrolmentID == request.EnrolmentID))
        {
            return Conflict(new { message = "A result already exists." });
        }

        var result = new Result
        {
            EnrolmentID = request.EnrolmentID,
            FinishTime = request.FinishTime,
            FinishPosition = request.FinishPosition,
            ResultDate = request.ResultDate ?? DateTime.UtcNow.Date
        };

        _context.Results.Add(result);
        await _context.SaveChangesAsync();

        return Created($"/api/Results/{result.ResultID}", result);
    }

    [Authorize(Roles = "Organiser")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResult(
        int id, ResultRequest request)
    {
        var result = await _context.Results
            .FirstOrDefaultAsync(r => r.ResultID == id);

        if (result == null)
            return NotFound();

        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.EnrolmentID == result.EnrolmentID);

        if (enrolment == null)
            return NotFound();

        var ownsEvent = await _context.Events.AnyAsync(e =>
            e.EventID == enrolment.EventID &&
            e.OrganiserID == CurrentUserId);

        if (!ownsEvent)
            return Forbid();

        if (request.EnrolmentID != result.EnrolmentID)
            return BadRequest(new
            {
                message = "The enrolment linked to a result cannot be changed."
            });

        result.FinishTime = request.FinishTime;
        result.FinishPosition = request.FinishPosition;
        result.ResultDate = request.ResultDate ?? DateTime.UtcNow.Date;

        await _context.SaveChangesAsync();
        return Ok(result);
    }

    [Authorize(Roles = "Organiser")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResult(int id)
    {
        var result = await _context.Results
            .FirstOrDefaultAsync(r => r.ResultID == id);

        if (result == null)
            return NotFound();

        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.EnrolmentID == result.EnrolmentID);

        if (enrolment == null)
            return NotFound();

        var ownsEvent = await _context.Events.AnyAsync(e =>
            e.EventID == enrolment.EventID &&
            e.OrganiserID == CurrentUserId);

        if (!ownsEvent)
            return Forbid();

        _context.Results.Remove(result);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class ResultRequest
{
    [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
    public int EnrolmentID { get; set; }

    public TimeSpan? FinishTime { get; set; }

    [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
    public int? FinishPosition { get; set; }

    public DateTime? ResultDate { get; set; }
}