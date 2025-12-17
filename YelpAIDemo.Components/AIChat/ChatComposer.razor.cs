using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace YelpAIDemo.YelpComponents.AIChat;

public partial class ChatComposer
{
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool IsStreaming { get; set; }

    [Parameter] public bool HasMessages { get; set; }

    [Parameter] public EventCallback<string> OnSend { get; set; }
    [Parameter] public EventCallback OnStop { get; set; }

    private readonly string[] _suggestions =
    {
        "Foodie weekend in Portland",
        "Family trip to San Diego",
        "Romantic Paris getaway"
    };

    private bool CanSend => !Disabled && !IsStreaming && !string.IsNullOrWhiteSpace(Value);

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (Disabled)
        {
            return;
        }

        if (e is { Key: "Enter", ShiftKey: false })
        {
            await Send();
        }
    }

    private async Task Send()
    {
        if (!CanSend)
        {
            return;
        }

        await OnSend.InvokeAsync(Value);
        Value = string.Empty;
        await ValueChanged.InvokeAsync(Value);
        StateHasChanged();
    }

    private async Task Stop()
    {
        if (!IsStreaming)
        {
            return;
        }

        await OnStop.InvokeAsync();
    }

    private async Task ApplySuggestion(string text)
    {
        Value = text;
        await ValueChanged.InvokeAsync(Value);
        StateHasChanged();
    }
}
