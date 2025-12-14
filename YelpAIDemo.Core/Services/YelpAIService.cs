using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.Core.Services;

public class YelpAIService(IConfiguration configuration, IHttpClientFactory clientFactory)
{
    private const string ChatEndpoint = "https://api.yelp.com/ai/chat/v2";
    private string? _yelpApiKey = configuration["YelpAPIKey"];

    public async Task<YelpAiResponse> SendRequestAsync(YelpAiRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_yelpApiKey))
        {
            Console.WriteLine("Yelp API key is not configured.");
            throw new InvalidOperationException("Yelp API key is not configured.");
        }
       
        using var httpClient = clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _yelpApiKey);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "YelpAI/1.0");
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, ChatEndpoint);
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        httpRequest.Content = content;
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        Console.WriteLine("ResponseBody:\n-----------------------------------------\n"+responseBody);
        return JsonSerializer.Deserialize<YelpAiResponse>(responseBody, new JsonSerializerOptions(){DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull});
    }
}