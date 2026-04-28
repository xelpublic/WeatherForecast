using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// данные о местоположении
/// </summary>
public class Location
{
    /// <summary>
    /// Название населённого пункта (город, посёлок и т. д.).
    /// Пример: "London" или "New York"
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Регион, штат или область. Для некоторых локаций может быть пустым.
    /// Пример: "England" или "New York" (штат)
    /// </summary>
    [JsonPropertyName("region")]
    public string Region { get; set; }

    /// <summary>
    /// Страна расположения.
    /// Пример: "United Kingdom" или "United States of America"
    /// </summary>
    [JsonPropertyName("country")]
    public string Country { get; set; }

    /// <summary>
    /// Широта местоположения в градусах (в диапазоне от −90 до 90).
    /// Положительное значение — северное полушарие, отрицательное — южное.
    /// Пример: 51.5085 (Лондон)
    /// </summary>
    [JsonPropertyName("lat")]
    public double? Latitude { get; set; }

    /// <summary>
    /// Долгота местоположения в градусах (в диапазоне от −180 до 180).
    /// Положительное значение — восточное полушарие, отрицательное — западное.
    /// Пример: −0.1257 (Лондон)
    /// </summary>
    [JsonPropertyName("lon")]
    public double? Longitude { get; set; }

    /// <summary>
    /// Временная зона местоположения в формате IANA.
    /// Используется для корректного отображения локального времени.
    /// Пример: "Europe/London" или "America/New_York"
    /// </summary>
    [JsonPropertyName("tz_id")]
    public string TimezoneId { get; set; }

    /// <summary>
    /// Смещение временной зоны относительно UTC в часах.
    /// Учитывает стандартное время и летнее время (DST), если оно применяется.
    /// Пример: 1.0 (UTC+1) для Лондона в период BST
    /// </summary>
    [JsonPropertyName("localtime_epoch")]
    public long? LocaltimeEpoch { get; set; }

    /// <summary>
    /// Локальное время в месте расположения в формате строки.
    /// Формат: "YYYY-MM-DD HH:mm"
    /// Пример: "2026-04-20 12:00"
    /// </summary>
    [JsonPropertyName("localtime")]
    public string Localtime { get; set; }

    /// <summary>
    /// Высота над уровнем моря в метрах.
    /// Поле может отсутствовать, если данные недоступны.
    /// Пример: 35 (метров)
    /// </summary>
    [JsonPropertyName("elevation_m")]
    public double? ElevationM { get; set; }
}