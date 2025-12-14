using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models.Bookings;

public sealed class OpeningsResponse
{
    [JsonPropertyName("reservation_times")]
    public List<OpeningsReservationDay> ReservationTimes { get; set; } = new();

    [JsonPropertyName("covers_range")]
    public CoversRange? CoversRange { get; set; }
}
public sealed class OpeningsReservationDay
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = default!; // "YYYY-MM-DD"

    [JsonPropertyName("times")]
    public List<OpeningsTimeSlot> Times { get; set; } = new();
}

public sealed class OpeningsTimeSlot
{
    [JsonPropertyName("credit_card_required")]
    public bool CreditCardRequired { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; } = default!; // "HH:mm"
}

public sealed class CoversRange
{
    [JsonPropertyName("min_party_size")]
    public int MinPartySize { get; set; }

    [JsonPropertyName("max_party_size")]
    public int MaxPartySize { get; set; }
}