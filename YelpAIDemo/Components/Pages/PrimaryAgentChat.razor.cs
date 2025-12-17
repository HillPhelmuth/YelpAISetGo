using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YelpAIDemo.Core;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Models.Chat;
using YelpAIDemo.Core.Services;
using YelpAIDemo.YelpComponents;
using YelpAIDemo.YelpComponents.AIChat;
using MEAIChatRole = Microsoft.Extensions.AI.ChatRole;

namespace YelpAIDemo.Components.Pages;

public partial class PrimaryAgentChat : IDisposable
{
    [Inject] private YelpAiAgentOrchestration Orchestration { get; set; } = default!;
    [Inject] private AppState AppState { get; set; } = default!;
    [Inject] private ITravelItineraryStore ItineraryStore { get; set; } = default!;
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;
    [Inject]
    private ReverseGeocodeService ReverseGeocodeService { get; set; } = default!;
    private LocationJsInterop LocationJsInterop => new(JsRuntime);
    private List<ChatMessage> Messages { get; } = [];
    private string Input { get; set; } = string.Empty;
    private bool IsBusy { get; set; }
    private CancellationTokenSource? _runCts;
    private Coordinates? _destination;
    private Tab _activeTab = Tab.Chat;
    private bool _newItinerary;
    private bool _isMenuOpen;
    protected override async Task OnInitializedAsync()
    {
        AppState.PropertyChanged += HandlePropertyChanged;
        Orchestration.MapRequested += HandleMapRequested;
        Orchestration.ChatMessageStoreLoaded += HandleChatMessageStoreLoaded;

        await base.OnInitializedAsync();
    }

    private async void HandleChatMessageStoreLoaded(object? sender, Microsoft.Agents.AI.ChatMessageStore e)
    {
        try
        {
            if (e is null)
            {
                Messages.Clear();
                return;
            }

            var messages = (await e.GetMessagesAsync())?.ToList() ?? [];
            if (messages.Count != 0) Messages.Clear();
            foreach (var message in messages.Where(message => message.Role == MEAIChatRole.Assistant || message.Role == MEAIChatRole.User))
            {
                if (message.Text.Contains("User Query:")) continue;
                Messages.Add(new ChatMessage()
                {
                    Role = message.Role == MEAIChatRole.Assistant ? ChatRole.Assistant : ChatRole.User,
                    Text = message.Text,
                    Timestamp = DateTime.Now
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something's gone terribly awry in `HandleChatMessageStoreLoaded`: {ex}");
        }
    }

    private void HandleMapRequested(object? sender, Coordinates coordinates)
    {
        Console.WriteLine($"Map requested: ({coordinates.Latitude}, {coordinates.Longitude})");
        _destination = coordinates;
        InvokeAsync(StateHasChanged);
    }

    private void HandleItineraryMapRequested(TravelItineraryItem item)
    {
        if (item?.Coordinates is null)
        {
            return;
        }

        _destination = item.Coordinates;
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var location = await LocationJsInterop.GetUserLocation();
            AppState.UserLocation = new Coordinates() { Latitude = location.Coords.Latitude, Longitude = location.Coords.Longitude };
            AppState.EstimatedUserAddress = await ReverseGeocodeService.ReverseGeocodeAsync(AppState.UserLocation.Latitude, AppState.UserLocation.Longitude);
            // AppState.LatestTravelItinerary ??= await ItineraryStore.LoadLatestAsync();

            //await SendAsync("Introduce yourself and briefly explain the process, or summarize our talk so far", true);
        }
    }

    private void HandlePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.LatestTravelItinerary))
        {
            _newItinerary = true;
        }
        InvokeAsync(StateHasChanged);
    }

    private void SetTab(Tab tab)
    {
        if (tab != Tab.Chat)
        {
            _isMenuOpen = false;
        }

        if (tab == Tab.Itinerary)
        {
            _newItinerary = false;
            StateHasChanged();
        }
        _activeTab = tab;
    }

    private void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
    }

    private string GetTabClass(Tab tab)
    {
        return _activeTab == tab ? "btn btn-sm btn-primary" : "btn btn-sm btn-outline-primary";
    }

    private async Task SendAsync(string input, bool skipAdd = false)
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
        if (!skipAdd)
        {
            Messages.Add(new ChatMessage
            {
                Role = ChatRole.User,
                Text = text,
                Timestamp = DateTimeOffset.Now
            });
        }
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
        var query = $"[User Location: {AppState.EstimatedUserAddress} (lat,lng): ({AppState.UserLocation?.Latitude}, {AppState.UserLocation?.Longitude})]\nUser Query: {text}";
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
        Orchestration.MapRequested -= HandleMapRequested;
        AppState.PropertyChanged -= HandlePropertyChanged;
    }
}

public enum Tab
{
    Chat,
    Itinerary
}
