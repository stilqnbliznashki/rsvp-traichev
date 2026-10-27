namespace Rsvp.Api.Models;

public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int EmployeeId { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public Customer Customer { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public Service Service { get; set; } = null!;
}
