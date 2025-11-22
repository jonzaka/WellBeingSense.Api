using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellBeingSense.Api.Data;
using WellBeingSense.Api.Models;

namespace WellBeingSense.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalCheckins = await _context.WellBeingCheckins.CountAsync();

        var stressHighCount = await _context.WellBeingCheckins
            .CountAsync(c => c.StressLevel == StressLevel.Alto);

        var byDepartment = await _context.WellBeingCheckins
            .Include(c => c.Employee)
            .GroupBy(c => c.Employee!.Department)
            .Select(g => new
            {
                Department = g.Key,
                AvgMood = g.Average(x => x.Mood),
                StressHighPercent = g.Count(x => x.StressLevel == StressLevel.Alto) * 100.0 / g.Count()
            })
            .ToListAsync();

        return Ok(new
        {
            TotalCheckins = totalCheckins,
            StressHighCount = stressHighCount,
            ByDepartment = byDepartment
        });
    }

    [HttpGet("alerts")]
    public async Task<ActionResult<IEnumerable<RiskAlert>>> GetAlerts()
    {
        var alerts = await _context.RiskAlerts
            .OrderByDescending(a => a.GeneratedAt)
            .ToListAsync();

        return alerts;
    }
}
