using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace YelpAIDemo.YelpComponents.AIChat;

public partial class ChatComposer
{
    [Parameter] public string Value { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> ValueChanged { get; set; }

    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool IsStreaming { get; set; }

    [Parameter] public EventCallback<string> OnSend { get; set; }
    [Parameter] public EventCallback OnStop { get; set; }

    private bool CanSend => !Disabled && !IsStreaming && !string.IsNullOrWhiteSpace(Value);

    private async Task OnInput(ChangeEventArgs e)
    {
        Value = e.Value?.ToString() ?? string.Empty;
        await ValueChanged.InvokeAsync(Value);
    }

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
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
}
