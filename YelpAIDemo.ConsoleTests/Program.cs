using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddHttpClient();
services.AddTransient<YelpAIService>();

using var serviceProvider = services.BuildServiceProvider();
var yelpAiService = serviceProvider.GetRequiredService<YelpAIService>();
var request = new YelpAiRequest()
{
    Query = "Kansas City Italian restaurant moderate upscale vegetarian options",
    UserContext = new LocationCoordinates() { Latitude = 38.928384, Longitude = -94.591386 }
};


Console.WriteLine("YelpAIService is ready to send requests. Add your query");
var query = Console.ReadLine();


// Remove User-Agent header
//content.Headers.Remove("User-Agent");

var response = await yelpAiService.SendRequestAsync(request);
Console.WriteLine("Yelp AI Response:");
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));