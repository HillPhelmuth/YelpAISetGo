using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace YelpAIDemo.Core.Models;

public class YelpAiResponse
{
    [Description("Unique identifier for the chat session")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("chat_id")]
    public string? ChatId { get; set; }

    [Description("Response content from Yelp AI")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("response")]
    public Response? Response { get; set; }

    [Description("Types of entities returned in the response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("types")]
    public List<string>? Types { get; set; }

    [Description("List of entities such as businesses returned by Yelp AI")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("entities")]
    public List<Entity>? Entities { get; set; }

    [Description("Request a quote survey information")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("request_a_quote_survey")]
    public RequestAQuoteSurvey? RequestAQuoteSurvey { get; set; }
}

public class Entity
{
    [Description("List of businesses in this entity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("businesses")]
    public List<Business>? Businesses { get; set; }
}

public class Business
{
    [Description("Unique business identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [Description("Business alias for URL-friendly identification")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    [Description("Business name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [Description("Yelp URL for the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("url")]
    public Uri? Url { get; set; }

    [Description("Physical location details of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("location")]
    public LocationCoordinates? Location { get; set; }

    [Description("Geographic coordinates of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("coordinates")]
    public Coordinates? Coordinates { get; set; }

    [Description("Total number of reviews for this business")]
    [JsonPropertyName("review_count")]
    public int ReviewCount { get; set; }

    [Description("Price range indicator (e.g., $, $$, $$$, $$$$)")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("price")]
    public string? Price { get; set; }

    [Description("Average rating of the business")]
    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [Description("Categories that describe the business type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("categories")]
    public List<Category>? Categories { get; set; }

    [Description("Business attributes and features")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("attributes")]
    public Attributes? Attributes { get; set; }

    [Description("Business phone number")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [Description("AI-generated summaries of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("summaries")]
    public Summaries? Summaries { get; set; }

    [Description("Contextual information relevant to the query")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("contextual_info")]
    public ContextualInfo? ContextualInfo { get; set; }
}

public class Attributes
{
    [Description("Whether the business accepts Apple Pay")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessAcceptsApplePay")]
    public bool? BusinessAcceptsApplePay { get; set; }

    [Description("Business website URL")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessUrl")]
    public Uri? BusinessUrl { get; set; }

    [Description("About this business bio photo dictionary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizBioPhotoDict")]
    public object? AboutThisBizBioPhotoDict { get; set; }

    [Description("Business recommendations from About This Biz section")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizBusinessRecommendation")]
    public List<object>? AboutThisBizBusinessRecommendation { get; set; }

    [Description("Alternate business address")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessAddressAlternate")]
    public object? BusinessAddressAlternate { get; set; }

    [Description("Standard Industrial Classification category")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessCategorySic")]
    public object? BusinessCategorySic { get; set; }

    [Description("Previous location if business has moved")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessMovedFrom")]
    public object? BusinessMovedFrom { get; set; }

    [Description("Alternate business name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessNameAlternate")]
    public object? BusinessNameAlternate { get; set; }

    [Description("Business opening date timestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessOpeningDate")]
    public int? BusinessOpeningDate { get; set; }

    [Description("Whether the business is temporarily closed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessTempClosed")]
    public bool? BusinessTempClosed { get; set; }

    [Description("Parent group name if business is part of a chain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("GroupName")]
    public object? GroupName { get; set; }

    [Description("Store code for chain locations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("StoreCode")]
    public object? StoreCode { get; set; }

    [Description("Biography about the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizBio")]
    public string? AboutThisBizBio { get; set; }

    [Description("First name of the business owner/representative")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizBioFirstName")]
    public string? AboutThisBizBioFirstName { get; set; }

    [Description("Last name of the business owner/representative")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizBioLastName")]
    public string? AboutThisBizBioLastName { get; set; }

    [Description("History of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizHistory")]
    public string? AboutThisBizHistory { get; set; }

    [Description("Role of the person in the About This Biz section")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizRole")]
    public string? AboutThisBizRole { get; set; }

    [Description("Business specialties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizSpecialties")]
    public string? AboutThisBizSpecialties { get; set; }

    [Description("Year the business was established")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AboutThisBizYearEstablished")]
    public string? AboutThisBizYearEstablished { get; set; }

    [Description("Type of alcohol service (e.g., full bar, beer & wine only)")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Alcohol")]
    public string? Alcohol { get; set; }

    [Description("Ambience characteristics of the venue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Ambience")]
    public Ambience? Ambience { get; set; }

    [Description("Whether bike parking is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BikeParking")]
    public bool? BikeParking { get; set; }

    [Description("Whether the business accepts Android Pay")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessAcceptsAndroidPay")]
    public bool? BusinessAcceptsAndroidPay { get; set; }

    [Description("Whether the business accepts Bitcoin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessAcceptsBitcoin")]
    public bool? BusinessAcceptsBitcoin { get; set; }

    [Description("Whether the business accepts credit cards")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessAcceptsCreditCards")]
    public bool? BusinessAcceptsCreditCards { get; set; }

    [Description("Display-friendly version of the business URL")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessDisplayUrl")]
    public Uri? BusinessDisplayUrl { get; set; }

    [Description("New location if business has moved")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessMovedTo")]
    public object? BusinessMovedTo { get; set; }

    [Description("Available parking options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessParking")]
    public BusinessParking? BusinessParking { get; set; }

    [Description("Whether the venue allows BYOB (Bring Your Own Bottle)")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BYOB")]
    public object? Byob { get; set; }

    [Description("BYOB corkage fee policy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BYOBCorkage")]
    public object? ByobCorkage { get; set; }

    [Description("Whether the business provides catering services")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Caters")]
    public bool? Caters { get; set; }

    [Description("Whether corkage fee applies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Corkage")]
    public bool? Corkage { get; set; }

    [Description("Whether dogs are allowed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DogsAllowed")]
    public bool? DogsAllowed { get; set; }

    [Description("Whether drive-thru service is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DriveThru")]
    public object? DriveThru { get; set; }

    [Description("Whether flower delivery service is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("FlowerDelivery")]
    public object? FlowerDelivery { get; set; }

    [Description("Whether gender-neutral restrooms are available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("GenderNeutralRestrooms")]
    public object? GenderNeutralRestrooms { get; set; }

    [Description("Whether the venue is good for kids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("GoodForKids")]
    public bool? GoodForKids { get; set; }

    [Description("Which meals the venue is good for")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("GoodForMeal")]
    public GoodForMeal? GoodForMeal { get; set; }

    [Description("Whether happy hour is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("HappyHour")]
    public object? HappyHour { get; set; }

    [Description("Whether TV is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("HasTV")]
    public bool? HasTv { get; set; }

    [Description("URL to the business menu")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MenuUrl")]
    public Uri? MenuUrl { get; set; }

    [Description("National Provider Identifier for healthcare businesses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NationalProviderIdentifier")]
    public object? NationalProviderIdentifier { get; set; }

    [Description("Noise level at the venue (e.g., quiet, average, loud)")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NoiseLevel")]
    public string? NoiseLevel { get; set; }

    [Description("Whether military discount is offered")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("OffersMilitaryDiscount")]
    public object? OffersMilitaryDiscount { get; set; }

    [Description("Ongoing vigilante event information")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("OngoingVigilanteEvent")]
    public object? OngoingVigilanteEvent { get; set; }

    [Description("Whether online reservations are available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("OnlineReservations")]
    public object? OnlineReservations { get; set; }

    [Description("Whether the business is open 24 hours")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Open24Hours")]
    public bool? Open24Hours { get; set; }

    [Description("Whether the business is open to all")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BusinessOpenToAll")]
    public bool? BusinessOpenToAll { get; set; }

    [Description("Platform delivery options")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PlatformDelivery")]
    public object? PlatformDelivery { get; set; }

    [Description("Whether there is a Pokéstop nearby")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PokestopNearby")]
    public object? PokestopNearby { get; set; }

    [Description("Whether counter service is available at the restaurant")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsCounterService")]
    public object? RestaurantsCounterService { get; set; }

    [Description("Whether the restaurant offers delivery")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsDelivery")]
    public bool? RestaurantsDelivery { get; set; }

    [Description("Whether the restaurant is good for groups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsGoodForGroups")]
    public bool? RestaurantsGoodForGroups { get; set; }

    [Description("Restaurant price range")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsPriceRange")]
    public object? RestaurantsPriceRange { get; set; }

    [Description("Restaurant price range on a scale of 1-4")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsPriceRange2")]
    public int? RestaurantsPriceRange2 { get; set; }

    [Description("Whether the restaurant accepts reservations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsReservations")]
    public bool? RestaurantsReservations { get; set; }

    [Description("Whether the restaurant offers table service")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsTableService")]
    public bool? RestaurantsTableService { get; set; }

    [Description("Whether the restaurant offers takeout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("RestaurantsTakeOut")]
    public bool? RestaurantsTakeOut { get; set; }

    [Description("Waitlist reservation information")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("WaitlistReservation")]
    public object? WaitlistReservation { get; set; }

    [Description("Whether the venue is wheelchair accessible")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("WheelchairAccessible")]
    public bool? WheelchairAccessible { get; set; }

    [Description("WiFi availability (e.g., free, paid, no)")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("WiFi")]
    public string? WiFi { get; set; }

    [Description("Long business summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("biz_summary_int")]
    public BizSummary? BizSummaryLong { get; set; }

    [Description("Business summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("biz_summary")]
    public BizSummary? BizSummary { get; set; }
}

public class Ambience
{
    [Description("Whether the ambience is divey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("divey")]
    public bool? Divey { get; set; }

    [Description("Whether the ambience is hipster")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hipster")]
    public bool? Hipster { get; set; }

    [Description("Whether the ambience is casual")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("casual")]
    public bool? Casual { get; set; }

    [Description("Whether the ambience is touristy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("touristy")]
    public bool? Touristy { get; set; }

    [Description("Whether the ambience is trendy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("trendy")]
    public bool? Trendy { get; set; }

    [Description("Whether the ambience is intimate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("intimate")]
    public bool? Intimate { get; set; }

    [Description("Whether the ambience is romantic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("romantic")]
    public bool? Romantic { get; set; }

    [Description("Whether the ambience is classy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("classy")]
    public bool? Classy { get; set; }

    [Description("Whether the ambience is upscale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("upscale")]
    public bool? Upscale { get; set; }
}

public class BizSummary
{
    [Description("Summary text of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [Description("Whether the summary is inactive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("is_inactive")]
    public bool? IsInactive { get; set; }

    [Description("Whether automatic summary generation is allowed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("allows_auto_summary_generation")]
    public bool? AllowsAutoSummaryGeneration { get; set; }

    [Description("Number of eligible reviews considered for the summary")]
    [JsonPropertyName("eligible_reviews_considered")]
    public int EligibleReviewsConsidered { get; set; }
}

public class BusinessParking
{
    [Description("Whether garage parking is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("garage")]
    public bool? Garage { get; set; }

    [Description("Whether street parking is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("street")]
    public bool? Street { get; set; }

    [Description("Whether validated parking is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("validated")]
    public bool? Validated { get; set; }

    [Description("Whether parking lot is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("lot")]
    public bool? Lot { get; set; }

    [Description("Whether valet parking is available")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("valet")]
    public bool? Valet { get; set; }
}

public class GoodForMeal
{
    [Description("Whether the venue is good for dessert")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("dessert")]
    public object? Dessert { get; set; }

    [Description("Whether the venue is good for late night dining")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("latenight")]
    public bool? Latenight { get; set; }

    [Description("Whether the venue is good for lunch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("lunch")]
    public bool? Lunch { get; set; }

    [Description("Whether the venue is good for dinner")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("dinner")]
    public bool? Dinner { get; set; }

    [Description("Whether the venue is good for brunch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("brunch")]
    public bool? Brunch { get; set; }

    [Description("Whether the venue is good for breakfast")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("breakfast")]
    public bool? Breakfast { get; set; }
}

public class Category
{
    [Description("Category alias identifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    [Description("Category title or name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

public class ContextualInfo
{
    [Description("Summary information relevant to the context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("summary")]
    public object? Summary { get; set; }

    [Description("Snippets from relevant reviews")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("review_snippets")]
    public List<ReviewSnippet> ReviewSnippets { get; set; }

    [Description("Business hours information")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("business_hours")]
    public List<object>? BusinessHours { get; set; }

    [Description("Relevant photos for the context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("photos")]
    public List<Photo>? Photos { get; set; }

    [Description("A single review snippet")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("review_snippet")]
    public string? ReviewSnippet { get; set; }

    [Description("Whether the business accepts reservations through Yelp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("accepts_reservations_through_yelp")]
    public bool? AcceptsReservationsThroughYelp { get; set; }

    [Description("Reservation availability information")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("reservation_availability")]
    public ReservationAvailability? ReservationAvailability { get; set; }
}
public class ReviewSnippet
{
    [JsonPropertyName("review_id")]
    public string? ReviewId { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("rating")]
    public long Rating { get; set; }
}
public class Photo
{
    [Description("Original URL of the photo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("original_url")]
    public Uri? OriginalUrl { get; set; }
}

public class ReservationAvailability
{
    [Description("Available reservation openings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("openings")]
    public List<Opening>? Openings { get; set; }
}

public class Opening
{
    [Description("Date of the reservation opening")]
    [JsonPropertyName("date")]
    public DateTimeOffset Date { get; set; }

    [Description("Available time slots for reservations")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("slots")]
    public List<Slot>? Slots { get; set; }
}

public class Slot
{
    [Description("Time of the reservation slot")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [Description("Available seating areas for this slot")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("seating_areas")]
    public List<string>? SeatingAreas { get; set; }
}

public class Coordinates
{
    [Description("Latitude coordinate")]
    [JsonPropertyName("latitude")]
    public float Latitude { get; set; }

    [Description("Longitude coordinate")]
    [JsonPropertyName("longitude")]
    public float Longitude { get; set; }
}

public class LocationCoordinates
{
    [Description("Primary street address")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [Description("Secondary address line (suite, unit, etc.)")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("address2")]
    public string? Address2 { get; set; }

    [Description("Tertiary address line")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("address3")]
    public string? Address3 { get; set; }

    [Description("City name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [Description("Postal/ZIP code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("zip_code")]
    public object? ZipCode { get; set; }

    [Description("State or province")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("state")]
    public string? State { get; set; }

    [Description("Country name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [Description("Full formatted address string")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("formatted_address")]
    public string? FormattedAddress { get; set; }
}

public class Summaries
{
    [Description("Short summary of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("short")]
    public string? Short { get; set; }

    [Description("Medium-length summary of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("medium")]
    public string? Medium { get; set; }

    [Description("Long/detailed summary of the business")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("int")]
    public string? Long { get; set; }
}

public class RequestAQuoteSurvey
{
    [Description("Display name of the job/service being requested")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("job_display_name")]
    public string? JobDisplayName { get; set; }

    [Description("Status of the quote request")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [Description("Survey questions for the quote request")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("questions")]
    public List<Question>? Questions { get; set; }

    [Description("User information for the quote request")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("user_info")]
    public UserInfo? UserInfo { get; set; }

    [Description("List of business IDs that have been requested")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("requested_biz_ids")]
    public object? RequestedBizIds { get; set; }

    [Description("Number of businesses matched for the quote request")]
    [JsonPropertyName("num_matched_businesses")]
    public int NumMatchedBusinesses { get; set; }

    [Description("Whether to submit the request to nearby businesses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("submit_to_nearby_businesses")]
    public bool? SubmitToNearbyBusinesses { get; set; }
}

public class Question
{
    [Description("Question text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [Description("List of available answer choices")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("available_choices")]
    public List<string>? AvailableChoices { get; set; }

    [Description("Whether only predefined choices are accepted")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("only_accepts_choices")]
    public bool? OnlyAcceptsChoices { get; set; }

    [Description("Whether multiple selections are allowed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("is_multi_select")]
    public bool? IsMultiSelect { get; set; }

    [Description("Whether the question is required")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("required")]
    public bool? QuestionRequired { get; set; }

    [Description("User's response to the question")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("user_response")]
    public List<string>? UserResponse { get; set; }
}

public class UserInfo
{
    [Description("User's first name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [Description("User's last name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [Description("User's email address")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [Description("User's phone number")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [Description("User's ZIP code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("zip_code")]
    public string? ZipCode { get; set; }
}

public class Response
{
    [Description("Response text from Yelp AI")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [Description("Tags associated with the response")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("tags")]
    public List<Tag>? Tags { get; set; }
}

public class Tag
{
    [Description("Type of tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("tag_type")]
    public string? TagType { get; set; }

    [Description("Start position of the tag in the text")]
    [JsonPropertyName("start")]
    public int Start { get; set; }

    [Description("End position of the tag in the text")]
    [JsonPropertyName("end")]
    public int End { get; set; }

    [Description("Metadata associated with the tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("meta")]
    public Meta? Meta { get; set; }
}

public class Meta
{
    [Description("Business ID associated with the metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("business_id")]
    public string? BusinessId { get; set; }
}