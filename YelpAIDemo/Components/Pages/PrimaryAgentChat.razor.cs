using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YelpAIDemo.Core;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;
using YelpAIDemo.YelpComponents;
using YelpAIDemo.YelpComponents.AIChat;

namespace YelpAIDemo.Components.Pages;

public partial class PrimaryAgentChat : IDisposable
{
    [Inject] private YelpAiAgentOrchestration Orchestration { get; set; } = default!;
    [Inject] private AppState AppState { get; set; } = default!;
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;
    private LocationJsInterop LocationJsInterop => new(JsRuntime);
    private List<ChatMessage> Messages { get; } = [];
    private string Input { get; set; } = string.Empty;
    private bool IsBusy { get; set; }
    private CancellationTokenSource? _runCts;
    private Coordinates? _destination;
    protected override Task OnInitializedAsync()
    {
        AppState.PropertyChanged += HandlePropertyChanged;
        Orchestration.MapRequested += HandleMapRequested;
        return base.OnInitializedAsync();
    }

    private void HandleMapRequested(object? sender, Coordinates coordinates)
    {
        _destination = coordinates;
        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var location = await LocationJsInterop.GetUserLocation();
            AppState.UserLocation = new Coordinates(){Latitude = location.Coords.Latitude, Longitude = location.Coords.Longitude};
        }
    }

    private void HandlePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    private async Task SendAsync(string input)
    {
        if (IsBusy)
        {
            return;
        }

        var text = input?.Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        //Input = string.Empty;

        Messages.Add(new ChatMessage
        {
            Role = ChatRole.User,
            Text = text,
            Timestamp = DateTimeOffset.Now
        });

        var assistantMessage = new ChatMessage
        {
            Role = ChatRole.Assistant,
            Text = string.Empty,
            Timestamp = DateTimeOffset.Now
        };
        Messages.Add(assistantMessage);

        IsBusy = true;
        _runCts?.Dispose();
        _runCts = new CancellationTokenSource();

        var lastRender = Environment.TickCount64;
        var buffer = new System.Text.StringBuilder();
        var query = $"[User Location (lat,lng): ({AppState.UserLocation?.Latitude}, {AppState.UserLocation?.Longitude})]\nUser Query: {text}";
        try
        {
            await foreach (var chunk in Orchestration.ExecuteAgentThreadAsync(query, _runCts.Token))
            {
                if (!string.IsNullOrEmpty(chunk))
                {
                    buffer.Append(chunk);
                    assistantMessage.Text = buffer.ToString();
                }

                var now = Environment.TickCount64;
                if (now - lastRender >= 75)
                {
                    lastRender = now;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }
        catch (OperationCanceledException)
        {
            if (assistantMessage.Text.Length == 0)
            {
                assistantMessage.Role = ChatRole.System;
                assistantMessage.Text = "Canceled.";
            }
        }
        catch (Exception ex)
        {
            assistantMessage.Role = ChatRole.Error;
            assistantMessage.Text = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void Stop()
    {
        if (!IsBusy)
        {
            return;
        }

        try
        {
            _runCts?.Cancel();
        }
        catch
        {
            // Ignore cancellation races.
        }
    }

    public void Dispose()
    {
        try
        {
            _runCts?.Cancel();
        }
        catch
        {
        }

        _runCts?.Dispose();
    }
}
