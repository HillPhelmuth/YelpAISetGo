using Microsoft.JSInterop;

namespace YelpAIDemo.YelpComponents;

public class LocationJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
        "import", "./_content/YelpAIDemo.Components/locationJsInterop.js").AsTask());

    public async ValueTask<LocationPosition> GetUserLocation()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<LocationPosition>("getUserLocation");
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
    public async ValueTask GenerateRoutesMap(float destinationLatitude, float destinationLongitude)
    {
        var origin = await GetUserLocation();
        var originObj = new { lat = origin.Coords.Latitude, lng = origin.Coords.Longitude };
        var destinationObj = new { lat = destinationLatitude, lng = destinationLongitude };
        await (await _moduleTask.Value).InvokeVoidAsync("initRoutesMap", originObj, destinationObj);
    }
}

public class LocationPosition
{
    public long Timestamp { get; set; }
    public Coords Coords { get; set; }
}

public class Coords
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    
}
