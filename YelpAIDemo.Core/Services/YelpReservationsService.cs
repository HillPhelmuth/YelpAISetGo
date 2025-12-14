using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using YelpAIDemo.Core.Models.Bookings;

namespace YelpAIDemo.Core.Services;

public interface IYelpReservationsService
{
    Task<OpeningsResponse> GetOpeningsAsync(
        string businessIdOrAlias,
        OpeningsRequest request,
        CancellationToken cancellationToken = default);

    Task<HoldResponse> CreateHoldAsync(
        string businessIdOrAlias,
        CreateHoldRequest request,
        CancellationToken cancellationToken = default);

    Task<ReservationResponse> CreateReservationAsync(
        string businessIdOrAlias,
        CreateReservationRequest request,
        CancellationToken cancellationToken = default);

    Task<ReservationStatusResponse> GetReservationStatusAsync(
        string reservationId,
        CancellationToken cancellationToken = default);

    Task CancelReservationAsync(
        string reservationId,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Typed client for Yelp Reservations "Bookings" endpoints:
/// - GET  /v3/bookings/{business_id_or_alias}/openings
/// - POST /v3/bookings/{business_id_or_alias}/holds
/// - POST /v3/bookings/{business_id_or_alias}/reservations
/// - GET  /v3/bookings/reservation/{reservation_id}/status
/// - POST /v3/bookings/reservation/{reservation_id}/cancel
/// 
/// HttpClient is expected to be configured with:
///   BaseAddress = https://api.yelp.com/v3/
///   DefaultRequestHeaders.Authorization = "Bearer {ACCESS_TOKEN}"
/// </summary>
public class YelpReservationsService : IYelpReservationsService
{
    private readonly HttpClient _httpClient;

    public YelpReservationsService(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _httpClient = clientFactory.CreateClient("Yelp");
        _httpClient.BaseAddress = new Uri("https://api.yelp.com/v3/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["YelpApiKey"]);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "YelpAI/1.0");
    }

    public async Task<OpeningsResponse> GetOpeningsAsync(
        string businessIdOrAlias,
        OpeningsRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(businessIdOrAlias))
            throw new ArgumentException("Business id or alias is required.", nameof(businessIdOrAlias));

        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var uriBuilder = new StringBuilder($"bookings/{Uri.EscapeDataString(businessIdOrAlias)}/openings?");
        var hasParams = false;

        if (request.Covers.HasValue)
        {
            uriBuilder.Append($"covers={request.Covers.Value.ToString(CultureInfo.InvariantCulture)}");
            hasParams = true;
        }

        if (request.Date.HasValue)
        {
            if (hasParams) uriBuilder.Append('&');
            var date = request.Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            uriBuilder.Append($"date={Uri.EscapeDataString(date)}");
            hasParams = true;
        }

        if (request.Time.HasValue)
        {
            if (hasParams) uriBuilder.Append('&');
            var time = request.Time.Value.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
            uriBuilder.Append($"time={Uri.EscapeDataString(time)}");
            hasParams = true;
        }

        var uri = uriBuilder.ToString();

        using var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OpeningsResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            throw new InvalidOperationException("Empty response from Yelp openings endpoint.");

        return result;
    }

    public async Task<HoldResponse> CreateHoldAsync(
        string businessIdOrAlias,
        CreateHoldRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(businessIdOrAlias))
            throw new ArgumentException("Business id or alias is required.", nameof(businessIdOrAlias));

        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var date = request.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var time = request.Time.ToString(@"hh\:mm", CultureInfo.InvariantCulture);

        var form = new Dictionary<string, string>
        {
            ["covers"] = request.Covers.ToString(CultureInfo.InvariantCulture),
            ["date"] = date,
            ["time"] = time,
            ["unique_id"] = request.UniqueId
        };

        using var content = new FormUrlEncodedContent(form);
        var uri = $"bookings/{Uri.EscapeDataString(businessIdOrAlias)}/holds";

        using var response = await _httpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<HoldResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            throw new InvalidOperationException("Empty response from Yelp holds endpoint.");

        return result;
    }

    public async Task<ReservationResponse> CreateReservationAsync(
        string businessIdOrAlias,
        CreateReservationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(businessIdOrAlias))
            throw new ArgumentException("Business id or alias is required.", nameof(businessIdOrAlias));

        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var date = request.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var time = request.Time.ToString(@"hh\:mm", CultureInfo.InvariantCulture);

        var form = new Dictionary<string, string>
        {
            ["covers"] = request.Covers.ToString(CultureInfo.InvariantCulture),
            ["date"] = date,
            ["time"] = time,
            ["unique_id"] = request.UniqueId,
            ["first_name"] = request.FirstName,
            ["last_name"] = request.LastName,
            ["email"] = request.Email,
            ["phone"] = request.Phone
        };

        if (!string.IsNullOrWhiteSpace(request.HoldId))
        {
            form["hold_id"] = request.HoldId!;
        }

        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            form["notes"] = request.Notes!;
        }

        using var content = new FormUrlEncodedContent(form);
        var uri = $"bookings/{Uri.EscapeDataString(businessIdOrAlias)}/reservations";

        using var response = await _httpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ReservationResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            throw new InvalidOperationException("Empty response from Yelp reservations endpoint.");

        return result;
    }

    public async Task<ReservationStatusResponse> GetReservationStatusAsync(
        string reservationId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reservationId))
            throw new ArgumentException("Reservation id is required.", nameof(reservationId));

        var uri = $"bookings/reservation/{Uri.EscapeDataString(reservationId)}/status";

        using var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ReservationStatusResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
            throw new InvalidOperationException("Empty response from Yelp reservation status endpoint.");

        return result;
    }

    public async Task CancelReservationAsync(
        string reservationId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reservationId))
            throw new ArgumentException("Reservation id is required.", nameof(reservationId));

        var uri = $"bookings/reservation/{Uri.EscapeDataString(reservationId)}/cancel";

        using var response = await _httpClient.PostAsync(uri, content: null, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        // Response body is {}, nothing to deserialize.
    }
}

