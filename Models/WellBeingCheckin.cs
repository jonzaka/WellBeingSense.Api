namespace WellBeingSense.Api.Models;

public class WellBeingCheckin
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public DateTime Timestamp { get; set; }

    public int Mood { get; set; }

    public StressLevel StressLevel { get; set; }

    public string Symptoms { get; set; } = string.Empty;
}
