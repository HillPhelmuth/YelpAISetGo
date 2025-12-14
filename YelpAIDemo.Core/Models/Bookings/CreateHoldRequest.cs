namespace YelpAIDemo.Core.Models.Bookings;

public sealed class CreateHoldRequest
{
    /// <summary>
    /// Number of covers (party size).
    /// </summary>
    public int Covers { get; init; }

    /// <summary>
    /// Desired reservation date (YYYY-MM-DD).
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// Desired reservation time (HH:mm, 24-hour).
    /// </summary>
    public TimeSpan Time { get; init; }

    /// <summary>
    /// Partner-generated unique id for the guest/session.
    /// </summary>
    public string UniqueId { get; init; } = default!;
}