using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBeingSense.Api.Data;
using WellBeingSense.Api.Models;
using WellBeingSense.Api.Services;

namespace WellBeingSense.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EnvironmentReadingsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RiskEvaluationService _riskService;

    public EnvironmentReadingsController(AppDbContext context, RiskEvaluationService riskService)
    {
        _context = context;
        _riskService = riskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnvironmentReading>>> GetAll()
    {
        return await _context.EnvironmentReadings
            .OrderByDescending(r => r.Timestamp)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<EnvironmentReading>> Create(EnvironmentReading reading)
    {
        reading.Timestamp = DateTime.UtcNow;
        _context.EnvironmentReadings.Add(reading);
        await _context.SaveChangesAsync();

        await _riskService.EvaluateEnvironmentAsync(reading);

        return CreatedAtAction(nameof(GetAll), new { id = reading.Id }, reading);
    }
}
