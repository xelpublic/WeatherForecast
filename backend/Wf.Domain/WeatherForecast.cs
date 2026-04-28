using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// Корневой класс прогноза погоды
/// </summary>
public class WeatherForecast
{
    [JsonPropertyName("location")]
    public Location Location { get; set; }
        
    [JsonPropertyName("current")]
    public Current Current { get; set; }
        
    [JsonPropertyName("forecast")]
    public Forecast Forecast { get; set; }
        
    [JsonPropertyName("alerts")]
    public Alerts Alerts { get; set; }
}