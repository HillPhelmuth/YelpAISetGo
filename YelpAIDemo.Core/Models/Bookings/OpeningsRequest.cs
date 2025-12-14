namespace YelpAIDemo.Core.Models.Bookings;

public sealed class OpeningsRequest
{
    /// <summary>
    /// Number of covers (party size).
    /// </summary>
    public int? Covers { get; init; }

    /// <summary>
    /// Desired reservation date (YYYY-MM-DD).
    /// </summary>
    public DateOnly? Date { get; init; }

    /// <summary>
    /// Desired reservation time (HH:mm, 24-hour).
    /// </summary>
    public TimeSpan? Time { get; init; }
}