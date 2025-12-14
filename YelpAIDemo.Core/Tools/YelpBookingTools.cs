using System;
using System.ComponentModel;
using System.Globalization;
using YelpAIDemo.Core.Models.Bookings;
using YelpAIDemo.Core.Services;

namespace YelpAIDemo.Core.Tools;

/// <summary>
/// Tool functions for Yelp Reservations (Bookings) endpoints.
///
/// These methods are intended to be registered as tools via:
///   AIFunctionFactory.Create(yelpBookingTools.SomeToolMethod)
///
/// Notes on parameters:
/// - Dates are passed as strings in ISO format: yyyy-MM-dd
/// - Times are passed as strings in 24-hour format: HH:mm
/// </summary>
public sealed class YelpBookingTools(YelpReservationsService bookingsClient)
{
	[Description("Get available reservation openings for a Yelp business (by business id or alias).")]
	public Task<OpeningsResponse> GetOpenings(
		[Description("Yelp business id or alias.")] string businessIdOrAlias,
		[Description("Optional party size (number of covers).")] int? covers = null,
		[Description("Optional date (yyyy-MM-dd).")] string? date = null,
		[Description("Optional time (HH:mm, 24-hour).")] string? time = null,
		CancellationToken cancellationToken = default)
	{
		var request = new OpeningsRequest
		{
			Covers = covers,
			Date = ParseDateOnlyOrNull(date),
			Time = ParseTimeSpanOrNull(time)
		};

		return bookingsClient.GetOpeningsAsync(businessIdOrAlias, request, cancellationToken);
	}

	[Description("Create a temporary hold for a reservation time at a Yelp business. Use the returned hold_id when creating a reservation.")]
	public Task<HoldResponse> CreateHold(
		[Description("Yelp business id or alias.")] string businessIdOrAlias,
		[Description("Party size (number of covers).")] int covers,
		[Description("Date (yyyy-MM-dd).")] string date,
		[Description("Time (HH:mm, 24-hour).")] string time,
		[Description("Partner-generated unique id for the guest/session.")] string uniqueId,
		CancellationToken cancellationToken = default)
	{
		var request = new CreateHoldRequest
		{
			Covers = covers,
			Date = ParseDateOnlyRequired(date, paramName: nameof(date)),
			Time = ParseTimeSpanRequired(time, paramName: nameof(time)),
			UniqueId = uniqueId
		};

		return bookingsClient.CreateHoldAsync(businessIdOrAlias, request, cancellationToken);
	}

	[Description("Create a reservation at a Yelp business. Optionally include hold_id from a prior hold.")]
	public Task<ReservationResponse> CreateReservation(
		[Description("Yelp business id or alias.")] string businessIdOrAlias,
		[Description("Party size (number of covers).")] int covers,
		[Description("Date (yyyy-MM-dd).")] string date,
		[Description("Time (HH:mm, 24-hour).")] string time,
		[Description("Partner-generated unique id for the guest/session.")] string uniqueId,
		[Description("Guest first name.")] string firstName,
		[Description("Guest last name.")] string lastName,
		[Description("Guest email.")] string email,
		[Description("Guest phone.")] string phone,
		[Description("Optional hold id returned by `CreateHold`.")] string? holdId = null,
		[Description("Optional reservation notes (occasion, special request, etc.).")] string? notes = null,
		CancellationToken cancellationToken = default)
	{
		var request = new CreateReservationRequest
		{
			Covers = covers,
			Date = ParseDateOnlyRequired(date, paramName: nameof(date)),
			Time = ParseTimeSpanRequired(time, paramName: nameof(time)),
			UniqueId = uniqueId,
			FirstName = firstName,
			LastName = lastName,
			Email = email,
			Phone = phone,
			HoldId = holdId,
			Notes = notes
		};

		return bookingsClient.CreateReservationAsync(businessIdOrAlias, request, cancellationToken);
	}

	[Description("Get the status for a Yelp reservation by reservation id.")]
	public Task<ReservationStatusResponse> GetReservationStatus(
		[Description("The Yelp reservation id.")] string reservationId,
		CancellationToken cancellationToken = default)
		=> bookingsClient.GetReservationStatusAsync(reservationId, cancellationToken);

	public sealed record CancelReservationResult(
		[property: Description("Whether the cancellation succeeded.")] bool Success,
		[property: Description("The reservation id that was cancelled.")] string ReservationId);

	[Description("Cancel a Yelp reservation by reservation id.")]
	public async Task<CancelReservationResult> CancelReservation(
		[Description("The Yelp reservation id.")] string reservationId,
		CancellationToken cancellationToken = default)
	{
		await bookingsClient.CancelReservationAsync(reservationId, cancellationToken).ConfigureAwait(false);
		return new CancelReservationResult(true, reservationId);
	}

	private static DateOnly? ParseDateOnlyOrNull(string? date)
	{
		if (string.IsNullOrWhiteSpace(date))
		{
			return null;
		}

		if (DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
		{
			return parsed;
		}

		throw new ArgumentException("Invalid date format. Expected yyyy-MM-dd.", nameof(date));
	}

	private static DateOnly ParseDateOnlyRequired(string date, string paramName)
	{
		if (DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
		{
			return parsed;
		}

		throw new ArgumentException("Invalid date format. Expected yyyy-MM-dd.", paramName);
	}

	private static TimeSpan? ParseTimeSpanOrNull(string? time)
	{
		if (string.IsNullOrWhiteSpace(time))
		{
			return null;
		}

		if (TimeSpan.TryParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture, out var parsed))
		{
			return parsed;
		}

		throw new ArgumentException("Invalid time format. Expected HH:mm (24-hour).", nameof(time));
	}

	private static TimeSpan ParseTimeSpanRequired(string time, string paramName)
	{
		if (TimeSpan.TryParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture, out var parsed))
		{
			return parsed;
		}

		throw new ArgumentException("Invalid time format. Expected HH:mm (24-hour).", paramName);
	}
}