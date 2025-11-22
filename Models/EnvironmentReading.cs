namespace WellBeingSense.Api.Models;

public class EnvironmentReading
{
    public int Id { get; set; }

    public string Area { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public double NoiseLevelDb { get; set; }
    public double TemperatureCelsius { get; set; }
    public double LuminosityLux { get; set; }
}
