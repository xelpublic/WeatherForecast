using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// прогноз погоды.
/// </summary>
public class Forecast
{
    [JsonPropertyName("forecastday")] public List<ForecastDay> ForecastDays { get; set; }
}