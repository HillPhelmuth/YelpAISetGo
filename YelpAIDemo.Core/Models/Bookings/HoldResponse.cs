using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models.Bookings;

public sealed class HoldResponse
{
    [JsonPropertyName("cancellation_policy")]
    public string? CancellationPolicy { get; set; }

    [JsonPropertyName("credit_card_hold")]
    public bool CreditCardHold { get; set; }

    /// <summary>
    /// Expiration timestamp in seconds since Unix epoch (as returned by Yelp).
    /// </summary>
    [JsonPropertyName("expires_at")]
    public double ExpiresAt { get; set; }

    [JsonPropertyName("hold_id")]
    public string HoldId { get; set; } = default!;

    [JsonPropertyName("is_editable")]
    public bool IsEditable { get; set; }

    /// <summary>
    /// Last cancellation timestamp in seconds since Unix epoch.
    /// </summary>
    [JsonPropertyName("last_cancellation_date")]
    public double LastCancellationDate { get; set; }

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("reserve_url")]
    public string? ReserveUrl { get; set; }
}