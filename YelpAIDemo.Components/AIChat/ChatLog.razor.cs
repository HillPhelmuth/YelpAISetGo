using Microsoft.AspNetCore.Components;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.YelpComponents.AIChat;
public partial class ChatLog
{
    [Parameter]
    [EditorRequired]
    public List<YelpAiResponse?> ChatResponses { get; set; } = [];
}
