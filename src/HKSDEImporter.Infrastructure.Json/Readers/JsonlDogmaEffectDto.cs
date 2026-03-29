using System.Text.Json;
using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlDogmaEffectDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("effectCategoryID")]
    public int? EffectCategoryId { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("guid")]
    public string? Guid { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("isOffensive")]
    public bool IsOffensive { get; init; }

    [JsonPropertyName("isAssistance")]
    public bool IsAssistance { get; init; }

    [JsonPropertyName("durationAttributeID")]
    public int? DurationAttributeId { get; init; }

    [JsonPropertyName("dischargeAttributeID")]
    public int? DischargeAttributeId { get; init; }

    [JsonPropertyName("rangeAttributeID")]
    public int? RangeAttributeId { get; init; }

    [JsonPropertyName("disallowAutoRepeat")]
    public bool DisallowAutoRepeat { get; init; }

    [JsonPropertyName("published")]
    public bool Published { get; init; }

    [JsonPropertyName("displayName")]
    public JsonlLocalizedTextDto? DisplayName { get; init; }

    [JsonPropertyName("isWarpSafe")]
    public bool IsWarpSafe { get; init; }

    [JsonPropertyName("rangeChance")]
    public bool RangeChance { get; init; }

    [JsonPropertyName("electronicChance")]
    public bool ElectronicChance { get; init; }

    [JsonPropertyName("propulsionChance")]
    public bool PropulsionChance { get; init; }

    [JsonPropertyName("distribution")]
    public int? Distribution { get; init; }

    [JsonPropertyName("modifierInfo")]
    public JsonElement? ModifierInfo { get; init; }
}
