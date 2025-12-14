namespace YelpAIDemo.Core.Models.Bookings;

public sealed class CreateReservationRequest
{
    /// <summary>
    /// Number of covers (party size).
    /// </summary>
    public int Covers { get; init; }

    /// <summary>
    /// Reservation date (must match the Hold if used).
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// Reservation time (must match the Hold if used).
    /// </summary>
    public TimeSpan Time { get; init; }

    /// <summary>
    /// Partner-generated unique id for the guest/session.
    /// </summary>
    public string UniqueId { get; init; } = default!;

    /// <summary>
    /// Optional hold id returned from the Holds endpoint.
    /// </summary>
    public string? HoldId { get; init; }

    public string FirstName { get; init; } = default!;
    public string LastName  { get; init; } = default!;
    public string Email     { get; init; } = default!;
    public string Phone     { get; init; } = default!;

    /// <summary>
    /// Optional reservation notes (e.g., occasion, special request).
    /// </summary>
    public string? Notes { get; init; }
}