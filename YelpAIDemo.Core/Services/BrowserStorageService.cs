using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.Agents.AI;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.Core.Services;

public class BrowserStorageService(ILocalStorageService protectedLocalStorage) : ITravelItineraryStore, IAgentStorageService
{
    public async Task SaveLatestAsync(TravelItinerary itinerary, CancellationToken cancellationToken = default)
    {
        var existing = await GetItineraries(cancellationToken);
        existing.Add(itinerary);
        await protectedLocalStorage.SetItemAsync("itineraries", existing, cancellationToken);
    }

    public async Task<TravelItinerary?> LoadLatestAsync(CancellationToken cancellationToken = default)
    {
        var existing = await GetItineraries(cancellationToken);
        return existing.OrderByDescending(i => i.CreatedAt).FirstOrDefault();
    }

    public async Task<List<TravelItinerary>> GetItineraries(CancellationToken cancellationToken = default)
    {
        return await protectedLocalStorage.GetItemAsync<List<TravelItinerary>>("itineraries", cancellationToken) ?? [];
    }
    public async Task SaveAgentThread(AgentThread agentThread, string itineraryName,
        CancellationToken cancellationToken = default)
    {
        var serializedThread = agentThread.Serialize();
        await protectedLocalStorage.SetItemAsync($"agentThread_{itineraryName}", serializedThread, cancellationToken);
    }
    public async Task<JsonElement?> LoadAgentThread(string itineraryName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(itineraryName)) return null;
        var hasThread = await protectedLocalStorage.ContainKeyAsync($"agentThread_{itineraryName}", cancellationToken);
        if (!hasThread) return null;
        var serializedThread = await protectedLocalStorage.GetItemAsync<JsonElement>($"agentThread_{itineraryName}", cancellationToken);
        return serializedThread;
    }
}