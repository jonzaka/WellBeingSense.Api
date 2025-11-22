namespace WellBeingSense.Api.Models;

public class RiskAlert
{
    public int Id { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public string Target { get; set; } = null!;

    public AlertSeverity Severity { get; set; }

    public string Message { get; set; } = null!;
}
