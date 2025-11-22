using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBeingSense.Api.Data;
using WellBeingSense.Api.Models;
using WellBeingSense.Api.Services;

namespace WellBeingSense.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WellBeingCheckinsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RiskEvaluationService _riskService;

    public WellBeingCheckinsController(AppDbContext context, RiskEvaluationService riskService)
    {
        _context = context;
        _riskService = riskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WellBeingCheckin>>> GetAll()
    {
        return await _context.WellBeingCheckins
            .Include(c => c.Employee)
            .OrderByDescending(c => c.Timestamp)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<WellBeingCheckin>> Create(WellBeingCheckin checkin)
    {
        checkin.Timestamp = DateTime.UtcNow;
        _context.WellBeingCheckins.Add(checkin);
        await _context.SaveChangesAsync();

        await _riskService.EvaluateCheckinAsync(checkin);

        return CreatedAtAction(nameof(GetAll), new { id = checkin.Id }, checkin);
    }
}
