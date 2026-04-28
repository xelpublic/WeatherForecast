using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// Корневой класс для десериализации блока alerts из JSON-ответа WeatherAPI
/// </summary>
public class Alerts
{
    [JsonPropertyName("alert")] public List<Alert> Alert { get; set; } = new List<Alert>();
}