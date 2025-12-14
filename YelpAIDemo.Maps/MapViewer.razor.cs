using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Location = YelpAIDemo.Core.Models.Coordinates;

namespace YelpAIDemo.Maps;
public partial class MapViewer
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    [Parameter]
    public float Latitude { get; set; }
    [Parameter]
    public float Longitude { get; set; }
    [Parameter]
    public bool Display { get; set; }

    private double _prevLat;
    private double _prevLon;
    private MapJsInterop MapsInterop => new(JsRuntime);
    protected override async Task OnParametersSetAsync()
    {

        if (Math.Abs(Latitude - _prevLat) > 0.001 && Math.Abs(Longitude - _prevLon) > 0.001)
        {
            _prevLat = Latitude;
            _prevLon = Longitude;
            await MapsInterop.GenerateRoutesMap(Latitude, Longitude);
        }
        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // This is where you can call JS functions after the component has rendered
            // For example, you might want to initialize a map or perform some other JS interop
            if (Math.Abs(Latitude - _prevLat) > 0.001 && Math.Abs(Longitude - _prevLon) > 0.001)
            {
                _prevLat = Latitude;
                _prevLon = Longitude;
                await MapsInterop.GenerateRoutesMap(Latitude, Longitude);
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }
}
