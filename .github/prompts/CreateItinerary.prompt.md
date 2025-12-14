---
agent: agent
---
# Travel Guide Agent Refactor

We're updating the application to as a broader travel guide rather than mere resturant recommender. All current functionality will remain, but will be supplimented with a toolset to create and persist a travel itinerary.

## Backend Updates

### Models and Tools

Add new models to `YelpAIDemo.Core\Models` for travel itineraries - times and locations that can be generated using existing models in `YelpAIDemo.Core\Models\YelpAIResponse.cs`. The parent class `TravelItinerary` will required `DescriptionAttribute`s on all properties as it will be used as structured output inside an `AITool` where a seperate agent will generate a `TravelItinerary`. The tool will look something like this:

```
[Description("Create a travel itinerary")]
public async Task<TravelItinerary> CreateItinerary([Description("instructions to create itinerary")] string instructions) // Note:this is only an example, actual input parameters might need to be more robust.
{
  // Set up the client, create instructions for the agent, use structured output (`ResponseFormat`)
  ...
  var chatClient = client.GetChatClient("openai/gpt-oss-120b").AsIChatClient();
var quickCreateAgent = chatClient.CreateAIAgent(
    options: new ChatClientAgentOptions()
    {
        
        Name = "Itinerary Agent",
        ChatOptions = new ChatOptions()
        {
            Instructions = $"{instructions}",
            ResponseFormat = ChatResponseFormat.ForJsonSchema<TravelItinerary>(),
            RawRepresentationFactory = _ => new OpenRouterChatCompletionOptions()
            {
                ReasoningEffortLevel = "high",
                Provider = new Provider() { Sort = "throughput" },
            }
        }
    });

var response = await quickCreateAgent.RunAsync<TravelItinerary>();
var itinerary = response.Result;
return itinerary;
}
```
### Prompt Instructions Updates

Update the prompt in `YelpAIDemo.Core\Models\Prompts.cs` to operate as instructions for a Yelp-based Travel Guide and include instructions for all available tools in `YelpAIDemo.Core\Tools` as well as a section for travel itinerary creation. The prompt should now include instructions on how to generate a travel itinerary based on user input.

## Frontend Updates

Add new components to `YelpAIDemo.Components\Itinerary` to display the travel itinerary. The components will include:
- `ItineraryDashboard.razor`: A dashboard component that displays a collection of travel itinerary items.
- `ItineraryCard.razor`: A card component that displays the travel itinerary item.

Add a new page to `YelpAIDemo\Components\Pages` to display the travel itinerary dashboard. 
