using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models;

public class Provider
{
    [JsonPropertyName("order"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Order { get; set; }
    [JsonPropertyName("sort"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Sort { get; set; }
}