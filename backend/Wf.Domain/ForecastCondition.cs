using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// данные о погодных условиях (текст, иконка, код)
/// </summary>
public class ForecastCondition
{
    /// <summary>
    /// Текстовое описание погодных условий на естественном языке.
    /// Может содержать как простые состояния («Sunny», «Cloudy»), так и вероятностные («Rain Likely», «Snow Possible»).
    /// Пример: "Partly cloudy" или "Thunderstorms Likely"
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    /// <summary>
    /// URL‑адрес иконки, визуально отображающей текущие погодные условия.
    /// Иконка адаптирована под дневное/ночное время (например, луна вместо солнца ночью).
    /// Пример: "https://example.com/icons/partly-cloudy-day.png"
    /// </summary>
    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    /// <summary>
    /// Цифровой код, однозначно идентифицирующий погодное условие.
    /// Используется для программной обработки и сопоставления с локальными справочниками иконок.
    /// Конкретные значения зависят от поставщика API.
    /// Пример: 1003 (соответствует «Partly cloudy»)
    /// </summary>
    [JsonPropertyName("code")]
    public int? Code { get; set; }
}