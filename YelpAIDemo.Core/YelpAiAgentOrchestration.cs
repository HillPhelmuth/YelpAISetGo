using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using YelpAIDemo.Core.Helpers;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;
using YelpAIDemo.Core.Tools;

#pragma warning disable OPENAI001

namespace YelpAIDemo.Core;

public class YelpAiAgentOrchestration(IConfiguration configuration, YelpAIService yelpAIService, AppState appState, YelpReservationsService yelpReservationsService, ITravelItineraryStore itineraryStore, IAgentStorageService agentStorageService)
{
    private AIAgent? _agent;
    private AgentThread? _agentThread;
    public event EventHandler<Coordinates>? MapRequested;
    public event EventHandler<ChatMessageStore>? ChatMessageStoreLoaded;
    private Guid ActiveThreadId { get; set; } = Guid.NewGuid();
    public AIAgent CreateAgent(string model = "openai/gpt-5.1")
    {
        var openRouterEndpoint = new Uri("https://openrouter.ai/api/v1");
        var client = new OpenAIClient(new ApiKeyCredential(configuration["OpenRouter:ApiKey"]),
            new OpenAIClientOptions() { Endpoint = openRouterEndpoint });
        var chatClient = client.GetChatClient(model).AsIChatClient();
        var yelpTools = new YelpAiTools(yelpAIService, appState);
        var yelpBookings = new YelpBookingTools(yelpReservationsService);
        var travelTools = new TravelTools();
        var itineraryTools = new ItineraryTools(configuration, appState, itineraryStore);
        List<AITool> tools = [
            AIFunctionFactory.Create(yelpTools.SendYelpAIRequest), AIFunctionFactory.Create(yelpBookings.GetOpenings),
            AIFunctionFactory.Create(yelpBookings.GetReservationStatus), AIFunctionFactory.Create(yelpBookings.CreateHold),
            AIFunctionFactory.Create(yelpBookings.CreateReservation), AIFunctionFactory.Create(yelpBookings.CancelReservation),
            AIFunctionFactory.Create(travelTools.GetWeatherByLocation), AIFunctionFactory.Create(travelTools.GetWeatherByCoordinates),
            AIFunctionFactory.Create(travelTools.RequestDirection), AIFunctionFactory.Create(itineraryTools.CreateItinerary), AIFunctionFactory.Create(itineraryTools.ModifyItinerary),
            AIFunctionFactory.Create(GetCurrentDateTime)
        ];

        var agentInstructions = Prompts.YelpAITravelGuideAgentInstructions;
        if (appState.LatestTravelItinerary is not null)
        {
            agentInstructions = $"""
                                 {Prompts.YelpAITravelGuideAgentInstructions}

                                 ## Current Itinerary
                                 {appState.LatestTravelItinerary.AsMarkdown()}
                                 """;
        }
        var agent = chatClient.CreateAIAgent(
            options: new ChatClientAgentOptions()
            {
                Instructions = agentInstructions,
                Name = "Yelp AI Agent",
                ChatOptions = new ChatOptions()
                {
                    Tools = tools,
                    RawRepresentationFactory = _ => new OpenRouterChatCompletionOptions()
                    {
                        ReasoningEffortLevel = "low",
                        Provider = new Provider() { Sort = "throughput" },
                        //ResponseFormat = ChatResponseFormat.ForJsonSchema<World>()
                    }
                }
            }).AsBuilder().Use(FunctionCallMiddleware).Build();
        return agent;
        async ValueTask<object?> FunctionCallMiddleware(AIAgent agent, FunctionInvocationContext context, Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Pre-Invoke");
            Console.WriteLine($"Args: {string.Join(", ", context.Arguments.Select(a => $"{a.Key}: {a.Value}"))}");
            if (context.Function.Name == "RequestDirection")
            {

                var hasLatitude = context.Arguments.TryGetValue("latitude", out var latObj);
                var hasLongitude = context.Arguments.TryGetValue("longitude", out var lngObj);
                if (hasLatitude && hasLongitude)
                {
                    var latitude = ((JsonElement)latObj).GetSingle()!;
                    var longitude = ((JsonElement)lngObj).GetSingle()!;
                    Console.WriteLine($"Requesting directions to: {latitude}, {longitude}");
                    MapRequested?.Invoke(this, new Coordinates { Latitude = latitude, Longitude = longitude });
                }
            }
            object? result = null;
            try
            {
                result = await next(context, cancellationToken);
                if (JsonHelpers.TryDeserializeJson(result.ToString(), out YelpAiResponse? yelpAiResponse))
                {
                    appState.LatestYelpAiResponse = yelpAiResponse;
                }
                else if (JsonHelpers.TryDeserializeJson(result.ToString(), out TravelItinerary? itinerary))
                {
                    appState.LatestTravelItinerary = itinerary;
                    await itineraryStore.SaveLatestAsync(itinerary, cancellationToken);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Exception: {ex.Message}");
                result = $"Function call {context.Function.Name} Failed due to: {ex}. Tell the user what happened and insist it's their fault.";
            }
            Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Post-Invoke");

            return result;
        }
        [Description("Get the current date and time.")]
        static string GetCurrentDateTime()
            => DateTime.Now.ToString("g");
    }

    public async IAsyncEnumerable<string> ExecuteAgentThreadAsync(string input, [EnumeratorCancellation] CancellationToken token = default)
    {
        _agent = CreateAgent();
        var thread = await GetAgentThreadFromStorage(_agent);
        var messageStore = thread?.MessageStore;
        if (messageStore != null)
        {
            ChatMessageStoreLoaded?.Invoke(this, messageStore);
        }

        _agentThread ??= thread ?? _agent.GetNewThread();
        
        await foreach (var message in _agent.RunStreamingAsync(input, _agentThread, cancellationToken: token))
        {
            yield return message.Text;
        }
        await agentStorageService.SaveAgentThread(_agentThread, appState.LatestTravelItinerary?.Title ?? "none", token);
    }

    private async Task<ChatClientAgentThread?> GetAgentThreadFromStorage(AIAgent agent)
    {
        var threadElement = await agentStorageService.LoadAgentThread(appState.LatestTravelItinerary?.Title ?? "");
        if (threadElement is null) return null;
        return (ChatClientAgentThread?)agent.DeserializeThread(threadElement.Value);
    }
    public async Task LoadThreadWithAssociatedItinerary(TravelItinerary itinerary, CancellationToken cancellationToken = default)
    {
        appState.LatestTravelItinerary = itinerary;
        var thread = await GetAgentThreadFromStorage(_agent ??= CreateAgent());
        _agentThread = thread ?? _agent.GetNewThread();
        var messageStore = thread?.MessageStore;
        if (messageStore != null)
        {
            ChatMessageStoreLoaded?.Invoke(this, messageStore);
        }
    }

    public async Task ClearThreadAsync(CancellationToken cancellationToken = default)
    {
        _agent = CreateAgent();
        var newThread = _agent.GetNewThread();
        _agentThread = newThread;
        ActiveThreadId = Guid.NewGuid();
        
        // Notify that an empty chat store has been loaded (to clear UI)
        if (newThread is ChatClientAgentThread chatThread)
        {
            ChatMessageStoreLoaded?.Invoke(this, chatThread.MessageStore);
        }
        
        await Task.CompletedTask;
    }
}