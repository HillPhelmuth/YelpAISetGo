using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;

#pragma warning disable OPENAI001

namespace YelpAIDemo.Core.Tools;

public sealed class ItineraryTools(IConfiguration configuration, AppState appState, ITravelItineraryStore itineraryStore)
{
    [Description("Create a travel itinerary. Use when the user asks for a day-by-day plan, schedule, or trip itinerary.")]
    public async Task<TravelItinerary> CreateItinerary(
        [Description("Instructions for creating an itinerary (interests, constraints, budget, pace, etc.).")] string instructions,
        [Description("List of businesses recommended by Yelp AI to include in the itinerary.")] List<BusinessSimple> businesses,
        [Description("Optional destination city/region.")] string destination,
        [Description("Optional start date (yyyy-MM-dd).")] string? startDate = null,
        [Description("Optional number of days.")] int? numberOfDays = null,
        [Description("Optional time zone (IANA preferred).")] string? timeZone = null,
        CancellationToken cancellationToken = default)
    {
        var openRouterEndpoint = new Uri("https://openrouter.ai/api/v1");
        var client = new OpenAIClient(
            new ApiKeyCredential(configuration["OpenRouter:ApiKey"]),
            new OpenAIClientOptions() { Endpoint = openRouterEndpoint });

        var chatClient = client.GetChatClient("openai/gpt-oss-120b").AsIChatClient();

        var toolInstructions = BuildItineraryAgentInstructions(instructions, destination, startDate, numberOfDays, timeZone, businesses);

        var itineraryAgent = chatClient.CreateAIAgent(
            options: new ChatClientAgentOptions()
            {
                Name = "Itinerary Agent",
                ChatOptions = new ChatOptions()
                {
                    Instructions = toolInstructions,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<TravelItinerary>(),
                    RawRepresentationFactory = _ => new OpenRouterChatCompletionOptions()
                    {
                        ReasoningEffortLevel = "high",
                        Provider = new Provider() { Sort = "throughput" },
                    }
                }
            });

        // The agent uses its own instructions and returns structured JSON.
        var result = await itineraryAgent.RunAsync<TravelItinerary>(cancellationToken: cancellationToken);
        var itinerary = result.Result ?? new TravelItinerary
        {
            Title = "Itinerary",
            Destination = destination,
            StartDate = startDate,
            TimeZone = timeZone,
            Summary = "null",
            Items = []
        };

        appState.LatestTravelItinerary = itinerary;
        await itineraryStore.SaveLatestAsync(itinerary, cancellationToken);

        return itinerary;
    }
    [Description("Modify a previously created itinerary.")]
    public string ModifyItinerary([Description("The modified itinerary. All items must be straight from Yelp AI recommendations")] TravelItinerary itinerary)
    {
        if (appState.LatestTravelItinerary is null)
        {
            return "Nope! You must first create an itinerary using the CreateItinerary tool.";
        }
        appState.LatestTravelItinerary = itinerary;
        _ = itineraryStore.SaveLatestAsync(itinerary);
        return "Itinerary updated successfully.";
    }
    private static string BuildItineraryAgentInstructions(string instructions, string? destination, string? startDate, int? numberOfDays, string? timeZone, List<BusinessSimple> businesses)
    {
        var sb = new StringBuilder();

        sb.AppendLine("You are a specialized travel itinerary planner.");
        sb.AppendLine("Return ONLY JSON that matches the provided schema.");
        sb.AppendLine("Do not include markdown, commentary, or code blocks.");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(destination))
        {
            sb.AppendLine($"Destination: {destination}");
        }

        if (!string.IsNullOrWhiteSpace(startDate))
        {
            sb.AppendLine($"StartDate (yyyy-MM-dd): {startDate}");
        }

        if (numberOfDays is not null)
        {
            sb.AppendLine($"NumberOfDays: {numberOfDays}");
        }

        if (!string.IsNullOrWhiteSpace(timeZone))
        {
            sb.AppendLine($"TimeZone: {timeZone}");
        }
        sb.AppendLine("**Businesses:**");
        foreach (var business in businesses)
        {
            sb.AppendLine(business.AsMarkdown());
            sb.AppendLine();
        }
        sb.AppendLine();
        sb.AppendLine("**Requirements:**");
        sb.AppendLine("- Produce an itinerary with a clear Title, Destination, Summary, and ordered Items using **only the provided businesses**.");
        sb.AppendLine("- Each item should have Day, StartTime/EndTime when applicable, Title, Description, and Category.");
        sb.AppendLine("- Include Location and Coordinates from the businesses.");
        sb.AppendLine("- If recommending a Yelp business, populate YelpBusinessIdOrAlias when possible, otherwise leave it null.");
        sb.AppendLine("- Keep timing realistic (transit buffers) and match user preferences.");
        sb.AppendLine();
        sb.AppendLine("User instructions:");
        sb.AppendLine(instructions);

        return sb.ToString();
    }
}
