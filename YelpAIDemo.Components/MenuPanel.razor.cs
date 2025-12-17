using Microsoft.AspNetCore.Components;
using YelpAIDemo.Core;
using YelpAIDemo.Core.Models;
using YelpAIDemo.Core.Services;

namespace YelpAIDemo.YelpComponents;

public partial class MenuPanel
{
	[Inject] private ITravelItineraryStore ItineraryStore { get; set; } = default!;
	[Inject] private AppState AppState { get; set; } = default!;
	[Inject] private YelpAiAgentOrchestration Orchestration { get; set; } = default!;
	
    private readonly List<TravelItinerary> _itineraries = [];
	private bool _isLoading;
	private bool _isSelecting;
	private bool _isClearing;
	private string? _error;
	private bool _includeAgentThread;
	
	protected override async Task OnInitializedAsync()
	{
		await ReloadAsync();
		await base.OnInitializedAsync();
	}

	private async Task ReloadAsync()
	{
		_error = null;
		_isLoading = true;
		try
		{
			_itineraries.Clear();
			var items = await ItineraryStore.GetItineraries();

			foreach (var itinerary in items
						 .Where(i => i is not null)
						 .OrderByDescending(i => i.CreatedAt))
			{
				_itineraries.Add(itinerary);
			}
		}
		catch (Exception ex)
		{
			_error = $"Failed to load itineraries: {ex.Message}";
		}
		finally
		{
			_isLoading = false;
		}
	}

	private bool IsActive(TravelItinerary itinerary)
	{
		var current = AppState.LatestTravelItinerary;
		if (current is null)
		{
			return false;
		}

		// Prefer matching on CreatedAt when possible (titles may repeat).
		if (!string.IsNullOrWhiteSpace(current.Title) || !string.IsNullOrWhiteSpace(itinerary.Title))
		{
			return string.Equals(current.Title, itinerary.Title, StringComparison.OrdinalIgnoreCase)
				   && current.CreatedAt == itinerary.CreatedAt;
		}

		return current.CreatedAt == itinerary.CreatedAt;
	}

	private async Task SelectAsync(TravelItinerary itinerary)
	{
		if (_isSelecting)
		{
			return;
		}

		_error = null;
		_isSelecting = true;
		try
		{
			// Loads itinerary into AppState and (optionally) restores the associated chat thread.
			if (_includeAgentThread)
                await Orchestration.LoadThreadWithAssociatedItinerary(itinerary);
			else 
                AppState.LatestTravelItinerary = itinerary;
        }
		catch (Exception ex)
		{
			_error = $"Failed to load itinerary: {ex.Message}";
		}
		finally
		{
			_isSelecting = false;
		}
	}

	private async Task ClearSessionAsync()
	{
		if (_isClearing)
		{
			return;
		}

		_error = null;
		_isClearing = true;
		try
		{
			// Clear the current itinerary
			AppState.LatestTravelItinerary = null;

			// Clear the agent thread and chat history
			await Orchestration.ClearThreadAsync();
		}
		catch (Exception ex)
		{
			_error = $"Failed to clear session: {ex.Message}";
		}
		finally
		{
			_isClearing = false;
		}
	}
}
