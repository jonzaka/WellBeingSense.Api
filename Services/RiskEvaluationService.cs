using WellBeingSense.Api.Data;
using WellBeingSense.Api.Models;

namespace WellBeingSense.Api.Services;

public class RiskEvaluationService
{
    private readonly AppDbContext _context;

    public RiskEvaluationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task EvaluateCheckinAsync(WellBeingCheckin checkin)
    {
        var employee = await _context.Employees.FindAsync(checkin.EmployeeId);
        if (employee is null) return;

        if (checkin.StressLevel == StressLevel.Alto && checkin.Mood <= 2)
        {
            var alert = new RiskAlert
            {
                Target = employee.Department,
                Severity = AlertSeverity.Alto,
                Message = $"Risco de burnout em {employee.Name} (departamento {employee.Department})."
            };

            _context.RiskAlerts.Add(alert);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EvaluateEnvironmentAsync(EnvironmentReading reading)
    {
        AlertSeverity? severity = null;
        var reasons = new List<string>();

        if (reading.NoiseLevelDb > 80)
        {
            severity = AlertSeverity.Alto;
            reasons.Add($"ruído acima de 80 dB ({reading.NoiseLevelDb:F1} dB)");
        }

        if (reading.TemperatureCelsius > 28)
        {
            severity ??= AlertSeverity.Medio;
            reasons.Add($"temperatura elevada ({reading.TemperatureCelsius:F1} °C)");
        }

        if (!reasons.Any()) return;

        var alert = new RiskAlert
        {
            Target = reading.Area,
            Severity = severity!.Value,
            Message = $"Condições ambientais críticas em {reading.Area}: {string.Join(", ", reasons)}."
        };

        _context.RiskAlerts.Add(alert);
        await _context.SaveChangesAsync();
    }
}
