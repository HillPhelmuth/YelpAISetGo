using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YelpAIDemo.Core.Models.Chat;

namespace YelpAIDemo.YelpComponents.AIChat;

public partial class ChatMessageList
{
    [Parameter] public IReadOnlyList<ChatMessage>? Messages { get; set; }
    [Inject]
    private IJSRuntime JS { get; set; } = null!;
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

    private static string MarkdownAsHtml(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var html = Markdown.ToHtml(text, pipeline);
        return html;
    }
    private ElementReference _messagesContainer;
    private bool _isAtBottom = true;
    private IJSObjectReference? _scrollModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _scrollModule = await JS.InvokeAsync<IJSObjectReference>(
                "import", "./_content/YelpAIDemo.Components/chatView.js");

            await _scrollModule.InvokeVoidAsync(
                "initializeScrollTracking", _messagesContainer,
                DotNetObjectReference.Create(this));
        }

        // Auto-scroll to bottom when new messages arrive
        if (_isAtBottom && Messages is not null && Messages.Any())
        {
            await ScrollToBottom();
        }
    }

    [JSInvokable]
    public void UpdateScrollPosition(bool isAtBottom)
    {
        _isAtBottom = isAtBottom;
        StateHasChanged();
    }

    private async Task ScrollToBottom()
    {
        if (_scrollModule != null)
        {
            await _scrollModule.InvokeVoidAsync("scrollToBottom", _messagesContainer);
            _isAtBottom = true;
        }
    }
}
