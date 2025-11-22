namespace WellBeingSense.Api.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Department { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<WellBeingCheckin> Checkins { get; set; } = new List<WellBeingCheckin>();
}
