using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models;

public class YelpAiRequest
{
    [JsonPropertyName("user_context"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Coordinates? UserContext { get; set; }

    [JsonPropertyName("query")]
    public required string Query { get; set; }

    [JsonPropertyName("chat_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ChatId { get; set; }
}
