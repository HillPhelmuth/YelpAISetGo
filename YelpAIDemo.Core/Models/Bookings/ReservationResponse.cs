using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models.Bookings;

public sealed class ReservationResponse
{
    [JsonPropertyName("confirmation_url")]
    public string ConfirmationUrl { get; set; } = default!;

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("reservation_id")]
    public string ReservationId { get; set; } = default!;
}