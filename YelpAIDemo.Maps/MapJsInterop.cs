using Microsoft.JSInterop;
using System.Text.Json;


namespace YelpAIDemo.Maps;
// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class MapJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
        "import", "./_content/YelpAIDemo.Maps/mapInterop.js").AsTask());

    public async ValueTask<string> Prompt(string message)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    public async ValueTask<GeolocationCoordinates?> GetCurrentLocation()
    {
        var item = await (await moduleTask.Value).InvokeAsync<string>("getLocation");
        try
        {
            Console.WriteLine($"Location data:\n=============================\n{item}");
            var coordinates = JsonSerializer.Deserialize<GeolocationCoordinates>(item);
            return coordinates;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting location: {ex.Message}");
            return null;
        }
    }

    public async ValueTask GenerateRoutesMap(float destinationLatitude, float destinationLongitude)
    {
        var origin = await GetCurrentLocation();
        var originObj = new { lat = origin.Latitude, lng = origin.Longitude };
        var destinationObj = new { lat = destinationLatitude, lng = destinationLongitude };
        await (await moduleTask.Value).InvokeVoidAsync("initRoutesMap", originObj, destinationObj);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}