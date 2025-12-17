using System.ComponentModel;
using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models;

public class BusinessSimple
{
    [Description("Unique business identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [Description("Business alias for URL-friendly identification")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("alias")]
    public required string Alias { get; set; }
    [Description("Short summary of the business")]
    public required string ShortSummary { get; set; }

    [Description("Business name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [Description("Yelp URL for the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("url")]
    public Uri? Url { get; set; }
    [Description("Geographic coordinates of the business. Absolutely required if available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("coordinates")]
    public required Coordinates? Coordinates { get; set; }
    [Description("Price range indicator (e.g., $, $$, $$$, $$$$)")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price")]
    public string? Price { get; set; }

    [Description("Average rating of the business")]
    [JsonPropertyName("rating")]
    public required double Rating { get; set; }
    [Description("Formatted address of the business")]
    public string? FormattedAddress { get; set; }
    [Description("Relevant photo for the business. Select one of the available photos in `contextual_info` Required if available.")]
    public required Photo? Photo { get; set; }

    public string AsMarkdown()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"## {Name}");
        sb.AppendLine($"- **Yelp ID**: {Id}");
        sb.AppendLine($"- **Alias**: {Alias}");
        sb.AppendLine($"- **Short Summary**: {ShortSummary}");
        sb.AppendLine($"- **URL**: {Url}");
        sb.AppendLine($"- **Coordinates**: Lat: {Coordinates?.Latitude}, Lon: {Coordinates?.Longitude}");
        sb.AppendLine($"- **Price**: {Price}");
        sb.AppendLine($"- **Rating**: {Rating}");
        sb.AppendLine($"- **Formatted Address**: {FormattedAddress}");
        sb.AppendLine($"- **Photo**: {Photo?.OriginalUrl}");
        return sb.ToString();
    }
}