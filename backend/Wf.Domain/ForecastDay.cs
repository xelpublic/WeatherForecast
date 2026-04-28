using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// Прогноз на один день.
/// </summary>
public class ForecastDay
{
    [JsonPropertyName("date")] public DateTime Date { get; set; }

    [JsonPropertyName("date_epoch")] public long DateEpoch { get; set; }

    [JsonPropertyName("day")] public ForecastDaySummary? Day { get; set; }

    [JsonPropertyName("astro")] public ForecastAstro? Astro { get; set; }

    [JsonPropertyName("hour")] public ForecastHour[] Hours { get; set; }
}