using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// Агрегированные дневные показатели погоды.
/// </summary>
public class ForecastDaySummary
{
    /// <summary>
    /// Максимальная температура за день в градусах Цельсия.
    /// Пример: 15.2
    /// </summary>
    [JsonPropertyName("maxtemp_c")]
    public double? MaxTempC { get; set; }

    /// <summary>
    /// Максимальная температура за день в градусах Фаренгейта.
    /// Пример: 59.4
    /// </summary>
    [JsonPropertyName("maxtemp_f")]
    public double? MaxTempF { get; set; }

    /// <summary>
    /// Минимальная температура за день в градусах Цельсия (обычно ночью).
    /// Пример: 8.7
    /// </summary>
    [JsonPropertyName("mintemp_c")]
    public double? MinTempC { get; set; }

    /// <summary>
    /// Минимальная температура за день в градусах Фаренгейта.
    /// Пример: 47.7
    /// </summary>
    [JsonPropertyName("mintemp_f")]
    public double? MinTempF { get; set; }

    /// <summary>
    /// Средняя температура за день в градусах Цельсия, рассчитанная по часовым данным.
    /// Пример: 12.1
    /// </summary>
    [JsonPropertyName("avgtemp_c")]
    public double? AvgTempC { get; set; }

    /// <summary>
    /// Средняя температура за день в градусах Фаренгейта.
    /// Пример: 53.8
    /// </summary>
    [JsonPropertyName("avgtemp_f")]
    public double? AvgTempF { get; set; }

    /// <summary>
    /// Максимальная скорость ветра за день в милях в час.
    /// Пример: 18.6
    /// </summary>
    [JsonPropertyName("maxwind_mph")]
    public double? MaxWindMph { get; set; }

    /// <summary>
    /// Максимальная скорость ветра за день в километрах в час.
    /// Пример: 30.0
    /// </summary>
    [JsonPropertyName("maxwind_kph")]
    public double? MaxWindKph { get; set; }

    /// <summary>
    /// Общее количество осадков за день в миллиметрах.
    /// Включает дождь, снег (пересчитанный в жидком эквиваленте) и другие осадки.
    /// Пример: 2.5
    /// </summary>
    [JsonPropertyName("totalprecip_mm")]
    public double? TotalPrecipMm { get; set; }

    /// <summary>
    /// Общее количество осадков за день в дюймах.
    /// Пример: 0.10
    /// </summary>
    [JsonPropertyName("totalprecip_in")]
    public double? TotalPrecipIn { get; set; }

    /// <summary>
    /// Общее количество снега за день в сантиметрах (непересчитанное).
    /// Поле может отсутствовать, если снега не было.
    /// Пример: 3.2
    /// </summary>
    [JsonPropertyName("totalsnow_cm")]
    public double? TotalSnowCm { get; set; }

    /// <summary>
    /// Средняя видимость за день в километрах.
    /// Показывает, насколько ясной была погода.
    /// Пример: 10.0 (максимальная видимость)
    /// </summary>
    [JsonPropertyName("avgvis_km")]
    public double? AvgVisKm { get; set; }

    /// <summary>
    /// Средняя видимость за день в милях.
    /// Пример: 6.2
    /// </summary>
    [JsonPropertyName("avgvis_miles")]
    public double? AvgVisMiles { get; set; }

    /// <summary>
    /// Средняя относительная влажность воздуха за день в процентах.
    /// Пример: 78 (умеренно влажно)
    /// </summary>
    [JsonPropertyName("avghumidity")]
    public double? AvgHumidity { get; set; }

    /// <summary>
    /// Флаг наличия дождя в течение дня (0 — дождя не будет, 1 — дождь ожидается).
    /// Бинарный индикатор для быстрого определения вероятности осадков.
    /// </summary>
    [JsonPropertyName("daily_will_it_rain")]
    public int? DailyWillItRain { get; set; }

    /// <summary>
    /// Процентная вероятность выпадения дождя в течение дня.
    /// Пример: 60 (60 % вероятность дождя)
    /// </summary>
    [JsonPropertyName("daily_chance_of_rain")]
    public int? DailyChanceOfRain { get; set; }

    /// <summary>
    /// Флаг наличия снега в течение дня (0 — снега не будет, 1 — снег ожидается).
    /// </summary>
    [JsonPropertyName("daily_will_it_snow")]
    public int? DailyWillItSnow { get; set; }

    /// <summary>
    /// Процентная вероятность выпадения снега в течение дня.
    /// Пример: 10 (10 % вероятность снега)
    /// </summary>
    [JsonPropertyName("daily_chance_of_snow")]
    public int? DailyChanceOfSnow { get; set; }

    /// <summary>
    /// Текстовое описание преобладающих погодных условий за день.
    /// Содержит текст, URL иконки и код условия.
    /// </summary>
    [JsonPropertyName("condition")]
    public ForecastCondition Condition { get; set; }

    /// <summary>
    /// Индекс ультрафиолетового излучения (UV Index) за день.
    /// Шкала от 0 (низкий риск) до 11+ (экстремальный риск).
    /// Пример: 3 (умеренный уровень)
    /// </summary>
    [JsonPropertyName("uv")]
    public double? Uv { get; set; }
}