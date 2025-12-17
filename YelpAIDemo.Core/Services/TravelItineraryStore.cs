using Microsoft.Agents.AI;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.Core.Services;

public interface ITravelItineraryStore
{
    Task SaveLatestAsync(TravelItinerary itinerary, CancellationToken cancellationToken = default);
    Task<TravelItinerary?> LoadLatestAsync(CancellationToken cancellationToken = default);
    Task<List<TravelItinerary>> GetItineraries(CancellationToken cancellationToken = default);
}
public interface IAgentStorageService
{
    Task SaveAgentThread(AgentThread agentThread, string itineraryName, CancellationToken cancellationToken = default);
    Task<JsonElement?> LoadAgentThread(string itineraryName, CancellationToken cancellationToken = default);
}