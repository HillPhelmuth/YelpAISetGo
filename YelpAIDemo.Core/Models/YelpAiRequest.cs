using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models;

public class YelpAiRequest
{
    [JsonPropertyName("user_context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LocationCoordinates? UserContext { get; set; }

    [JsonPropertyName("query")]
    public required string Query { get; set; }

    [JsonPropertyName("chat_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ChatId { get; set; }
}

public class LocationCoordinates
{
    [JsonPropertyName("latitude")]
    public float Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public float Longitude { get; set; }
}