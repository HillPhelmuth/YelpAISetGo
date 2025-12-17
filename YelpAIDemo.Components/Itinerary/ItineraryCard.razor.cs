using Microsoft.AspNetCore.Components;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.YelpComponents.Itinerary;

public partial class ItineraryCard
{
    [Parameter, EditorRequired] public TravelItineraryItem Item { get; set; } = default!;
    [Parameter]
    public EventCallback<TravelItineraryItem> MapToItemRequested { get; set; }

    private void GetDirections()
    {
        MapToItemRequested.InvokeAsync(Item);
    }
}