using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models;

public class Provider
{
    [JsonPropertyName("type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Type { get; set; }
    [JsonPropertyName("sort"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Sort { get; set; }
}