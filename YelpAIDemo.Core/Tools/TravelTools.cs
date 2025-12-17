using System.ComponentModel;
using System.Text;
using OpenMeteo;

namespace YelpAIDemo.Core.Tools;

public class TravelTools
{
    private OpenMeteoClient _client = new();

    [Description("Gets the current weather for a location. Use this tool only if you do not have latitude and longitude for the user location")]
    public async Task<string> GetWeatherByLocation(
        [Description(
            "Pass US Zipcode, UK Postcode, Canada Postalcode, IP address, or city name to get current weather for that location")]
        string location)
    {
        location = Uri.EscapeDataString(location);
        var weatherData = await _client.QueryAsync(location, new WeatherForecastOptions { Temperature_Unit = TemperatureUnitType.fahrenheit, Current = CurrentOptions.All, Daily = DailyOptions.All});
        var weather = weatherData.ToMarkdown(location);
        return weather;
    }
    [Description("Gets the current weather for a location specified by latitude and longitude")]
    public async Task<string> GetWeatherByCoordinates(
        [Description("Latitude of the location")] float latitude,
        [Description("Longitude of the location")] float longitude)
    {
        var weatherData = await _client.QueryAsync(new WeatherForecastOptions { Temperature_Unit = TemperatureUnitType.fahrenheit, Current = CurrentOptions.All, Daily = DailyOptions.All, Latitude = latitude, Longitude = longitude});
        var location = $"({latitude}, {longitude})";
        var weather = weatherData.ToMarkdown(location);
        return weather;
    }
    [Description("Requests map directions to a specified location")]
    public string RequestDirection([Description("Latitude of the destination")] float latitude,[Description("Logitude of the destination")] float longitude,[Description("Description of the destination")] string destinationDescription)
    {
        var response = $"Requesting map directions to {destinationDescription} at coordinates ({latitude}, {longitude}). Result will display in the UI.";
        return response;
    }
}
internal static class WeatherPluginExtensions
{
    extension(WeatherForecast weatherForecast)
    {
        public string ToMarkdown(string location)
        {
            var weather = weatherForecast.Current;
            var weatherForecastCurrentUnits = weatherForecast.CurrentUnits!;
            var sb = new StringBuilder();
            sb.AppendLine($"The weather in {location} is:");
            sb.AppendLine($"- Weather Condition: {weather.Weathercode?.WeathercodeDescription()}");
            sb.AppendLine($"- Temperature: {weather.Temperature}{weatherForecastCurrentUnits.Temperature}");
            sb.AppendLine($"- Feels Like: {weather.Apparent_temperature}{weatherForecastCurrentUnits.Apparent_temperature}");
            sb.AppendLine($"- Wind Speed: {weather.Windspeed_10m} {weatherForecastCurrentUnits.Windspeed_10m}");
            sb.AppendLine($"- Wind Direction: {weather.Winddirection_10m} {weatherForecastCurrentUnits.Winddirection_10m}");
            sb.AppendLine($"- Humidity: {weather.Relativehumidity_2m} {weatherForecastCurrentUnits.Relativehumidity_2m}");
            sb.AppendLine($"- Precipitation: {weather.Precipitation} {weatherForecastCurrentUnits.Precipitation}");
            sb.AppendLine($"- Pressure: {weather.Pressure_msl} {weatherForecastCurrentUnits.Pressure_msl}");
            sb.AppendLine("");
            sb.AppendLine($"{weatherForecast.ConvertWeatherForecastToMarkdownTable()}");
            return sb.ToString();
        }

        public string ConvertWeatherForecastToMarkdownTable()
        {
            var sb = new StringBuilder();
            sb.AppendLine("| Date       | Condition | Max Temp (°F) | Min Temp (°F) | Apparent Max Temp (°F) | Apparent Min Temp (°F) | Precipitation (mm) | Wind Speed (mph) | Sunrise  | Sunset  |");
            sb.AppendLine("|------------|---------------|---------------|---------------|------------------------|------------------------|--------------------|------------------|----------|---------|");

            for (int i = 0; i < weatherForecast.Daily.Time.Length; i++)
            {
                sb.AppendLine($"| {weatherForecast.Daily.Time[i]} | {weatherForecast.Daily.Weathercode?[i].WeathercodeDescription()} | {weatherForecast.Daily.Temperature_2m_max?[i]} | {weatherForecast.Daily.Temperature_2m_min?[i]} | {weatherForecast.Daily.Apparent_temperature_max?[i]} | {weatherForecast.Daily.Apparent_temperature_min?[i]} | {weatherForecast.Daily.Precipitation_sum?[i]} | {weatherForecast.Daily.Windspeed_10m_max?[i]} | {(weatherForecast.Daily.Sunrise?[i])?[11..]} | {(weatherForecast.Daily.Sunset?[i])?[11..]} |");
            }

            return sb.ToString();
        }
    }

    public static float ToFahrenheit(this float? celsius)
    {
        if (celsius == null)
            return 0;
        return celsius.Value * 9 / 5 + 32;
    }

    public static string WeathercodeDescription(this float weathercode)
    {
        if (weathercode == null)
            return "Invalid weathercode";
        var code = (int)weathercode;
        return code.WeathercodeDescription();
    }
    public static string WeathercodeDescription(this int weathercode)
    {
        //var weathercode = current.Weathercode;
        return weathercode switch
        {
            0 => "Clear sky",
            1 => "Mainly clear",
            2 => "Partly cloudy",
            3 => "Overcast",
            45 => "Fog",
            48 => "Depositing rime Fog",
            51 => "Light drizzle",
            53 => "Moderate drizzle",
            55 => "Dense drizzle",
            56 => "Light freezing drizzle",
            57 => "Dense freezing drizzle",
            61 => "Slight rain",
            63 => "Moderate rain",
            65 => "Heavy rain",
            66 => "Light freezing rain",
            67 => "Heavy freezing rain",
            71 => "Slight snow fall",
            73 => "Moderate snow fall",
            75 => "Heavy snow fall",
            77 => "Snow grains",
            80 => "Slight rain showers",
            81 => "Moderate rain showers",
            82 => "Violent rain showers",
            85 => "Slight snow showers",
            86 => "Heavy snow showers",
            95 => "Thunderstorm",
            96 => "Thunderstorm with light hail",
            99 => "Thunderstorm with heavy hail",
            _ => "Invalid weathercode"
        };
    }
}