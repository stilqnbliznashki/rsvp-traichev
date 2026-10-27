namespace Rsvp.Api.Models;

public class WorkingHour
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartsAt { get; set; }
    public TimeOnly EndsAt { get; set; }

    public Employee Employee { get; set; } = null!;
}
