using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;
using YelpAIDemo.Core.Tools;

#pragma warning disable OPENAI001

namespace YelpAIDemo.Core;

public class YelpAiAgentOrchestration(IConfiguration configuration, YelpAIService yelpAIService, AppState appState, YelpReservationsService yelpReservationsService)
{
    private AIAgent? _agent;
    private AgentThread? _agentThread;
    public event EventHandler<Coordinates>? MapRequested;
    public AIAgent CreateAgent(string model = "openai/gpt-oss-120b")
    {
        var openRouterEndpoint = new Uri("https://openrouter.ai/api/v1");
        var client = new OpenAIClient(new ApiKeyCredential(configuration["OpenRouter:ApiKey"]),
            new OpenAIClientOptions() { Endpoint = openRouterEndpoint });
        var chatClient = client.GetChatClient(model).AsIChatClient();
        var yelpTools = new YelpAiTools(yelpAIService, appState);
        var yelpBookings = new YelpBookingTools(yelpReservationsService);
        var travelTools = new TravelTools();
        List<AITool> tools = [
            AIFunctionFactory.Create(yelpTools.SendYelpAIRequest), AIFunctionFactory.Create(yelpBookings.GetOpenings), 
            AIFunctionFactory.Create(yelpBookings.GetReservationStatus), AIFunctionFactory.Create(yelpBookings.CreateHold), 
            AIFunctionFactory.Create(yelpBookings.CreateReservation), AIFunctionFactory.Create(yelpBookings.CancelReservation),
            AIFunctionFactory.Create(travelTools.GetWeatherByLocation), AIFunctionFactory.Create(travelTools.GetWeatherByCoordinates),
            AIFunctionFactory.Create(travelTools.RequestDirection)
        ];

        var agent = chatClient.CreateAIAgent(
            options: new ChatClientAgentOptions()
            {
                Instructions = Prompts.YelpAITravelGuideAgentInstructions,
                Name = "Yelp AI Agent",
                ChatOptions = new ChatOptions()
                {
                    Tools = tools,
                    RawRepresentationFactory = _ => new OpenRouterChatCompletionOptions()
                    {
                        ReasoningEffortLevel = "high",
                        Provider = new Provider() { Sort = "throughput" },
                        //ResponseFormat = ChatResponseFormat.ForJsonSchema<World>()
                    }
                }
            }).AsBuilder().Use(FunctionCallMiddleware).Build();
        return agent;
        async ValueTask<object?> FunctionCallMiddleware(AIAgent agent, FunctionInvocationContext context, Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Pre-Invoke");
            if (context.Function.Name == "RequestDirection")
            {
                var hasLatitude = context.Arguments.TryGetValue("latitude", out var latObj) && latObj is float;
                var hasLongitude = context.Arguments.TryGetValue("longitude", out var lngObj) && lngObj is float;
                if (hasLatitude && hasLongitude)
                {
                    var latitude = (float)latObj!;
                    var longitude = (float)lngObj!;
                    MapRequested?.Invoke(this, new Coordinates{ Latitude = latitude, Longitude = longitude });
                }
            }
            object? result = null;
            try
            {
                result = await next(context, cancellationToken);
                appState.LatestYelpAiResponse = (YelpAiResponse)result!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Exception: {ex.Message}");
                result = $"Function call {context.Function.Name} Failed due to: {ex}. Tell the player what happened and insist it's their fault.";
            }
            Console.WriteLine($"Function Name: {context!.Function.Name} - Middleware 1 Post-Invoke");

            return result;
        }
    }

    public async IAsyncEnumerable<string> ExecuteAgentThreadAsync(string input, [EnumeratorCancellation] CancellationToken token = default)
    {
        _agent ??= CreateAgent();
        _agentThread ??= _agent.GetNewThread();
        await foreach (var message in _agent.RunStreamingAsync(input, _agentThread, cancellationToken: token))
        {
            yield return message.Text;
        }
    }
}