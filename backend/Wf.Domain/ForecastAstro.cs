using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// Астрономические данные на день.(восход/закат солнца и луны)
/// </summary>
public class ForecastAstro
{
    /// <summary>
    /// Время восхода солнца в локальном времени местоположения.
    /// Формат: HH:mm (12‑часовой формат).
    /// Пример: "05:45"
    /// </summary>
    [JsonPropertyName("sunrise")]
    public string Sunrise { get; set; }

    /// <summary>
    /// Время захода солнца в локальном времени местоположения.
    /// Формат: HH:mm (12‑часовой формат).
    /// Пример: "19:30"
    /// </summary>
    [JsonPropertyName("sunset")]
    public string Sunset { get; set; }

    /// <summary>
    /// Время восхода луны в локальном времени местоположения.
    /// Может быть пустым в дни новолуния или при отсутствии восхода в течение суток.
    /// Формат: HH:mm (12‑часовой формат).
    /// Пример: "19:30"
    /// </summary>
    [JsonPropertyName("moonrise")]
    public string Moonrise { get; set; }

    /// <summary>
    /// Время захода луны в локальном времени местоположения.
    /// Может быть пустым в дни полнолуния или при отсутствии захода в течение суток.
    /// Формат: HH:mm (12‑часовой формат).
    /// Пример: "19:30"
    /// </summary>
    [JsonPropertyName("moonset")]
    public string Moonset { get; set; }

    /// <summary>
    /// Фаза луны на день. Описывает видимую часть освещённой поверхности.
    /// Возможные значения: "New Moon", "Waxing Crescent", "First Quarter",
    /// "Waxing Gibbous", "Full Moon", "Waning Gibbous",
    /// "Last Quarter", "Waning Crescent".
    /// Пример: "Waxing Crescent"
    /// </summary>
    [JsonPropertyName("moon_phase")]
    public string MoonPhase { get; set; }

    /// <summary>
    /// Освещённость луны в процентах — доля освещённой видимой поверхности.
    /// Диапазон: от 0 % (новолуние) до 100 % (полнолуние).
    /// Пример: 25 (первая четверть)
    /// </summary>
    [JsonPropertyName("moon_illumination")]
    public int? MoonIllumination { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_moon_up")]
    public int? IsMoonUp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("is_sun_up")]
    public int? IsSunUp { get; set; }
}