using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.YelpComponents.Maps;
public partial class MapViewer
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    
    [Parameter]
    public bool Display { get; set; }
    [Parameter]
    public Coordinates? Destination { get; set; }

    private float _prevLat;
    private float _prevLon;
    private LocationJsInterop MapsInterop => new(JsRuntime);
    protected override async Task OnParametersSetAsync()
    {

        if (Destination is not null && Math.Abs(Destination.Latitude - _prevLat) > 0.001 && Math.Abs(Destination.Longitude - _prevLon) > 0.001)
        {
            _prevLat = Destination.Latitude;
            _prevLon = Destination.Longitude;
            await MapsInterop.GenerateRoutesMap(Destination.Latitude, Destination.Longitude);
        }
        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // This is where you can call JS functions after the component has rendered
            // For example, you might want to initialize a map or perform some other JS interop
            if (Destination is not null && Math.Abs(Destination.Latitude - _prevLat) > 0.001 && Math.Abs(Destination.Longitude - _prevLon) > 0.001)
            {
                _prevLat = Destination.Latitude;
                _prevLon = Destination.Longitude;
                await MapsInterop.GenerateRoutesMap(Destination.Latitude, Destination.Longitude);
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
