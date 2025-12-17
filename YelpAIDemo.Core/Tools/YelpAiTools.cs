using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;

namespace YelpAIDemo.Core.Tools;

public class YelpAiTools(YelpAIService yelpAIService, AppState appState)
{
    [Description(YelpAiToolDescription)]
    public async Task<string> SendYelpAIRequest([Description("The user's query for Yelp AI or response to the Yelp AI follow-up questions")]string query,[Description("The chat session indicator received from the service. Optional by default, but required after the first interaction for successful multi-turn sessions. **Note:** Do not include unless you received a session id from the service already.")] string? chatSessionId = null)
    {
        var userLocation = appState.UserLocation;
        var request = new YelpAiRequest() { Query = query, ChatId = chatSessionId, UserContext = userLocation };
        if (string.IsNullOrEmpty(chatSessionId))
            request.ChatId = null;
        var response = await yelpAIService.SendRequestAsync(request);
        return JsonSerializer.Serialize(response, new JsonSerializerOptions {DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull});
    }

    private const string YelpAiToolDescription =
        """
        Combines multi-turn conversational intelligence with Yelp's latest business data and reviews.
        
        **Yelp AI Features**
        
        - Next generation search & discovery - Search with natural language, discover, and connect with contextually relevant businesses.
        - Multi-turn conversations - Support back-and-forth interactions and refine queries with follow-up questions.
        - Direct business queries - Ask targeted questions about businesses without needing to perform a prior search.
        - Instant quotes for home services - Simply describe the service you need (like “I need quotes for fixing a leaking faucet”), and Yelp AI instantly connects you with top local pros, so you receive multiple quotes from the best professionals for your job. (Requires multi-turn chat for end-to-end process)
        """;
}