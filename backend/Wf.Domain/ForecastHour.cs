using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// Детальный почасовой прогноз.
/// </summary>
public class ForecastHour
{
    /// <summary>
    /// Время прогноза в локальном времени местоположения.
    /// Формат: "YYYY-MM-DD HH:mm"
    /// Пример: "2026-04-20 12:00"
    /// </summary>
    [JsonPropertyName("time")]
    public string Time { get; set; }

    /// <summary>
    /// Временная метка Unix (количество секунд с 1 января 1970 года, UTC).
    /// Используется для программной обработки и сравнения временных точек.
    /// Пример: 1745164800
    /// </summary>
    [JsonPropertyName("time_epoch")]
    public long? TimeEpoch { get; set; }

    /// <summary>
    /// Температура воздуха в градусах Цельсия.
    /// Пример: 14.5
    /// </summary>
    [JsonPropertyName("temp_c")]
    public double? TempC { get; set; }

    /// <summary>
    /// Температура воздуха в градусах Фаренгейта.
    /// Пример: 58.1
    /// </summary>
    [JsonPropertyName("temp_f")]
    public double? TempF { get; set; }

    /// <summary>
    /// Ощущаемая температура в градусах Цельсия (с учётом ветра и влажности).
    /// Показывает, насколько тепло или холодно ощущается на самом деле.
    /// Пример: 13.2
    /// </summary>
    [JsonPropertyName("feelslike_c")]
    public double? FeelsLikeC { get; set; }

    /// <summary>
    /// Ощущаемая температура в градусах Фаренгейта.
    /// Пример: 55.8
    /// </summary>
    [JsonPropertyName("feelslike_f")]
    public double? FeelsLikeF { get; set; }

    /// <summary>
    /// Относительная влажность воздуха в процентах.
    /// Диапазон: 0–100 %.
    /// Пример: 65 (умеренная влажность)
    /// </summary>
    [JsonPropertyName("humidity")]
    public int? Humidity { get; set; }

    /// <summary>
    /// Процент облачности (доля неба, покрытая облаками).
    /// Диапазон: 0–100 %. 0 % — ясное небо, 100 % — полностью облачное.
    /// Пример: 80
    /// </summary>
    [JsonPropertyName("cloud")]
    public int? Cloud { get; set; }

    /// <summary>
    /// Видимость в километрах.
    /// Показывает максимальное расстояние, на котором различимы объекты.
    /// Пример: 10.0 (хорошая видимость)
    /// </summary>
    [JsonPropertyName("vis_km")]
    public double? VisKm { get; set; }

    /// <summary>
    /// Видимость в милях.
    /// Пример: 6.2
    /// </summary>
    [JsonPropertyName("vis_miles")]
    public double? VisMiles { get; set; }

    /// <summary>
    /// Атмосферное давление в миллибарах.
    /// Нормальное давление: ~1013 мбар.
    /// Пример: 1015.3
    /// </summary>
    [JsonPropertyName("pressure_mb")]
    public double? PressureMb { get; set; }

    /// <summary>
    /// Атмосферное давление в дюймах ртутного столба.
    /// Пример: 29.98
    /// </summary>
    [JsonPropertyName("pressure_in")]
    public double? PressureIn { get; set; }

    /// <summary>
    /// Количество осадков за последний час в миллиметрах.
    /// Включает дождь, снег (в жидком эквиваленте) и другие осадки.
    /// Пример: 0.2
    /// </summary>
    [JsonPropertyName("precip_mm")]
    public double? PrecipMm { get; set; }

    /// <summary>
    /// Количество осадков за последний час в дюймах.
    /// Пример: 0.01
    /// </summary>
    [JsonPropertyName("precip_in")]
    public double? PrecipIn { get; set; }

    /// <summary>
    /// Количество снега за последний час в сантиметрах.
    /// Поле может отсутствовать, если снега не было.
    /// Пример: 0.5
    /// </summary>
    [JsonPropertyName("snow_cm")]
    public double? SnowCm { get; set; }

    /// <summary>
    /// Скорость ветра в милях в час.
    /// Пример: 12.4
    /// </summary>
    [JsonPropertyName("wind_mph")]
    public double? WindMph { get; set; }

    /// <summary>
    /// Скорость ветра в километрах в час.
    /// Пример: 20.0
    /// </summary>
    [JsonPropertyName("wind_kph")]
    public double? WindKph { get; set; }

    /// <summary>
    /// Направление ветра в градусах (0° = север, 90° = восток, 180° = юг, 270° = запад).
    /// Пример: 225 (юго‑запад)
    /// </summary>
    [JsonPropertyName("wind_degree")]
    public int? WindDegree { get; set; }

    /// <summary>
    /// Текстовое обозначение направления ветра.
    /// Возможные значения: "N", "NNE", "NE", "ENE", "E" и т. д.
    /// Пример: "SW" (юго‑запад)
    /// </summary>
    [JsonPropertyName("wind_dir")]
    public string WindDir { get; set; }

    /// <summary>
    /// Порыв ветра в милях в час (максимальная скорость ветра за период).
    /// Пример: 18.6
    /// </summary>
    [JsonPropertyName("gust_mph")]
    public double? GustMph { get; set; }

    /// <summary>
    /// Порыв ветра в километрах в час.
    /// Пример: 30.0
    /// </summary>
    [JsonPropertyName("gust_kph")]
    public double? GustKph { get; set; }

    /// <summary>
    /// Индекс ультрафиолетового излучения (UV Index) для данного часа.
    /// Шкала от 0 (низкий риск) до 11+ (экстремальный риск).
    /// Пример: 4 (умеренный уровень)
    /// </summary>
    [JsonPropertyName("uv")]
    public double? Uv { get; set; }

    /// <summary>
    /// Текстовое описание погодных условий для данного часа.
    /// Содержит текст, URL иконки и код условия.
    /// </summary>
    [JsonPropertyName("condition")]
    public ForecastCondition Condition { get; set; }

    /// <summary>
    /// Вероятность осадков в процентах для данного часа.
    /// Пример: 30 (30 % вероятность дождя или снега в этот час)
    /// </summary>
    [JsonPropertyName("chance_of_rain")]
    public int? ChanceOfRain { get; set; }

    /// <summary>
    /// Вероятность выпадения снега в процентах для данного часа.
    /// Пример: 5 (5 % вероятность снега в этот час)
    /// </summary>
    [JsonPropertyName("chance_of_snow")]
    public int? ChanceOfSnow { get; set; }

    /// <summary>
    /// Показатель, указывающий, является ли данный час днём (1) или ночью (0).
    /// Определяется по положению солнца относительно горизонта.
    /// Пример: 1 (день)
    /// </summary>
    [JsonPropertyName("is_day")]
    public int? IsDay { get; set; }
}