using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Services;

public class ReverseGeocodeService(IHttpClientFactory clientFactory)
{
    public async Task<string> ReverseGeocodeAsync(float lat, float lon)
    {
        using var client = clientFactory.CreateClient();

        // Set required headers per Nominatim usage policy
        client.DefaultRequestHeaders.UserAgent.ParseAdd($"{GetType()}/1.0 (+https://myapp.example.com; contact@myapp.example.com)");

        //var queryString = string.Join("&", query.Select(kvp =>
        //    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

        var url = $"https://nominatim.openstreetmap.org/reverse?lat={lat}&lon={lon}&format=json&zoom=15";

        var response = await client.GetStringAsync(url);
        Console.WriteLine($"ReverseGeoCode response:\n{response}");
        var geoCodeReversed = JsonSerializer.Deserialize<ReverseGeo>(response);
        return geoCodeReversed?.DisplayName ?? "";
    }
}
public class ReverseGeo
{
    [JsonPropertyName("place_id")]
    public long PlaceId { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("address")]
    public Address? Address { get; set; }

}

public class Address
{

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("county")]
    public string? County { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }
}