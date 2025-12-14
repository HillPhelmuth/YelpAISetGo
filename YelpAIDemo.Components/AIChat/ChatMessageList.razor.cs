using Microsoft.AspNetCore.Components;

namespace YelpAIDemo.YelpComponents.AIChat;

public partial class ChatMessageList
{
    [Parameter] public IReadOnlyList<ChatMessage>? Messages { get; set; }

    private static string GetRowClass(ChatMessage msg) => msg.Role switch
    {
        ChatRole.User => "is-user",
        _ => "is-agent"
    };

    private static string GetBubbleClass(ChatMessage msg) => msg.Role switch
    {
        ChatRole.System => "is-system",
        ChatRole.User => "is-user",
        ChatRole.Assistant => "is-assistant",
        ChatRole.Error => "is-error",
        _ => ""
    };
}
