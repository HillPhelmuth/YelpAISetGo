using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models;

public class OpenRouterChatCompletionOptions : OpenAI.Chat.ChatCompletionOptions
{
    [JsonPropertyName("provider"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Provider? Provider { get; set; }
}