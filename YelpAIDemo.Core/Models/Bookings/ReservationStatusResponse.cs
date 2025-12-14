using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models.Bookings;

public sealed class ReservationStatusResponse
{
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; } = default!; // "YYYY-MM-DD"

    [JsonPropertyName("time")]
    public string Time { get; set; } = default!; // e.g. "11:00:00"

    [JsonPropertyName("covers")]
    public int Covers { get; set; }
}