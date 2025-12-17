using Microsoft.AspNetCore.Components;
using YelpAIDemo.Core.Models;

namespace YelpAIDemo.YelpComponents.Itinerary;

public partial class ItineraryDashboard
{
    [Parameter] public TravelItinerary? Itinerary { get; set; }
    [Parameter]
    public EventCallback<TravelItineraryItem> MapToItemRequested { get; set; }
}