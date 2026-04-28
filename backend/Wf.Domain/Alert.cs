using System.Text.Json.Serialization;

namespace Wf.Domain;

/// <summary>
/// погодное предупреждения
/// </summary>
public class Alert
{
    /// <summary>
    /// Краткий информативный заголовок предупреждения, отображаемый в первую очередь.
    /// Пример: "Severe Thunderstorm Warning"
    /// </summary>
    [JsonPropertyName("headline")]
    public string Headline { get; set; }


    /// <summary>
    /// Тип погодного явления, вызывающего предупреждение.
    /// Возможные значения: "Thunderstorm", "Flood", "Winter Storm",
    /// "High Wind", "Tornado" и т. д.
    /// </summary>
    [JsonPropertyName("event")]
    public string Event { get; set; }


    /// <summary>
    /// Время начала действия предупреждения в формате ISO 8601 (UTC).
    /// Пример: "2026-04-20T14:30:00+00:00"
    /// </summary>
    [JsonPropertyName("effective")]
    public string Effective { get; set; }


    /// <summary>
    /// Время окончания действия предупреждения в формате ISO 8601 (UTC).
    /// После этой даты предупреждение считается недействительным.
    /// Пример: "2026-04-20T20:00:00+00:00"
    /// </summary>
    [JsonPropertyName("expires")]
    public string Expires { get; set; }


    /// <summary>
    /// Подробное текстовое описание погодного явления и его потенциальных последствий.
    /// Может содержать технические детали, прогнозы интенсивности, ожидаемую продолжительность.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }


    /// <summary>
    /// Конкретные инструкции и рекомендации по действиям для населения.
    /// Указывает, что следует делать для обеспечения безопасности.
    /// Пример: "Seek shelter indoors immediately. Avoid downed power lines."
    /// </summary>
    [JsonPropertyName("instruction")]
    public string Instruction { get; set; }


    /// <summary>
    /// Категория предупреждения по классификации системы оповещения.
    /// Типичные значения: "Met" (метеорологическое), "Safety", "Security", "Rescue".
    /// Помогает определить сферу применения предупреждения.
    /// </summary>
    [JsonPropertyName("category")]
    public string Category { get; set; }


    /// <summary>
    /// Уровень серьёзности погодного явления.
    /// Возможные значения (по убыванию):
    /// - "Extreme" — экстремальная угроза жизни и имуществу;
    /// - "Severe" — серьёзная угроза;
    /// - "Moderate" — умеренная угроза;
    /// - "Minor" — незначительная угроза.
    /// </summary>
    [JsonPropertyName("severity")]
    public string Severity { get; set; }


    /// <summary>
    /// Степень уверенности метеорологов в реализации данного сценария.
    /// Возможные значения:
    /// - "Observed" — явление уже наблюдается;
    /// - "Likely" — высокая вероятность (более 50 %);
    /// - "Possible" — возможная реализация (менее 50 %).
    /// </summary>
    [JsonPropertyName("certainty")]
    public string Certainty { get; set; }


    /// <summary>
    /// Срочность реагирования на предупреждение.
    /// Возможные значения:
    /// - "Immediate" — требуется немедленное действие;
    /// - "Expected" — действие ожидается в ближайшее время;
    /// - "Future" — событие ожидается в отдалённой перспективе;
    /// - "Past" — событие уже произошло.
    /// </summary>
    [JsonPropertyName("urgency")]
    public string Urgency { get; set; }

    /// <summary>
    /// Географические зоны, на которые распространяется предупреждение.
    /// Может содержать список районов, округов, городов или координат.
    /// Пример: "Central London, Westminster, City of London"
    /// </summary>
    [JsonPropertyName("areas")]
    public string Areas { get; set; }

    /// <summary>
    /// Дополнительная информация или примечания от метеорологической службы.
    /// Может включать ссылки на источники, уточнения по прогнозу или контактные данные.
    /// Поле может быть пустым.
    /// </summary>
    [JsonPropertyName("note")]
    public string Note { get; set; }

    /// <summary>
    /// Тип сообщения в рамках жизненного цикла оповещения.
    /// Возможные значения:
    /// - "Alert" — первоначальное предупреждение;
    /// - "Update" — обновление существующего предупреждения;
    /// - "Cancel" — отмена ранее выданного предупреждения.
    /// Позволяет отслеживать эволюцию ситуации.
    /// </summary>
    [JsonPropertyName("msg_type")]
    public string MsgType { get; set; }
}