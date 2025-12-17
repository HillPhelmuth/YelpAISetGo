using Microsoft.AspNetCore.Components;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;

namespace YelpAIDemo.YelpComponents.AIChat;
public partial class ChatLog
{
    [Parameter]
    [EditorRequired]
    public List<YelpAiResponse?> ChatResponses { get; set; } = [];
    [Inject]
    private AppState AppState { get; set; } = default!;
}
