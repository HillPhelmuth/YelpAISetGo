using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YelpAIDemo.Core.Models;

public sealed class TravelItinerary
{
    [Description("A short title for this itinerary.")]
    public required string Title { get; set; }

    [Description("The primary destination (city/region) for the itinerary.")]
    public required string Destination { get; set; }

    [Description("An overall summary of the itinerary and the travel style.")]
    public required string Summary { get; set; }

    [Description("Start date in yyyy-MM-dd when provided.")]
    public string? StartDate { get; set; }

    [Description("End date in yyyy-MM-dd when provided.")]
    public string? EndDate { get; set; }

    [Description("The time zone relevant to scheduled times (IANA preferred, e.g. America/Los_Angeles).")]
    public string? TimeZone { get; set; }

    [Description("Ordered list of itinerary items (activities, meals, transit, lodging, etc.).")]
    public required List<TravelItineraryItem> Items { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string AsMarkdown()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"**{Title}**");
        if (!string.IsNullOrWhiteSpace(Summary))
        {
            sb.AppendLine();
            sb.AppendLine(Summary);
        }

        if (!string.IsNullOrEmpty(Destination))
        {
            sb.AppendLine();
            sb.AppendLine($"**Destination:** {Destination}");
        }

        foreach (var item in Items)
        {
            sb.AppendLine();
            sb.AppendLine($"**{item.Day}**");
            sb.AppendLine($"**{nameof(item.Title)}:** {item.Title}");
            sb.AppendLine($"**{nameof(item.Description)}:** {item.Description}");
        }

        return sb.ToString();
    }
}


public sealed class TravelItineraryItem
{
    [Description("A label for the day (e.g. Day 1, Day 2) or a date (yyyy-MM-dd).")]
    public required string Day { get; set; }

    [Description("Start time in local time (HH:mm) when applicable.")]
    public required string StartTime { get; set; }

    [Description("End time in local time (HH:mm) when applicable.")]
    public required string EndTime { get; set; }

    [Description("Short name of the stop or activity.")]
    public required string Title { get; set; }

    [Description("What to do here and why it's a good fit for the user's preferences.")]
    public required string Description { get; set; }

    [Description("Category for the item (e.g. Food, Activity, Lodging, Transit, Shopping).")]
    public required string Category { get; set; }

    [Description("Yelp business id or alias this must added straight from a provided business.")]
    public required string YelpBusinessIdOrAlias { get; set; }

    [Description("Optional human-readable address info for the location.")]
    public LocationCoordinates? Location { get; set; }

    [Description("latitude/longitude for mapping. Required if available.")]
    public Coordinates? Coordinates { get; set; }

    [Description("Any extra notes like booking tips, timing cautions, or alternatives.")]
    public string? Notes { get; set; }
    [Description("Url for the associated photo.")]
    public required string PhotoUrl { get; set; }
}
