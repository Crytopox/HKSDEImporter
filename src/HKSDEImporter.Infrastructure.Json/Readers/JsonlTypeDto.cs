using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlTypeDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("groupID")]
    public int GroupId { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("soundID")]
    public int? SoundId { get; init; }

    [JsonPropertyName("graphicID")]
    public int? GraphicId { get; init; }

    [JsonPropertyName("marketGroupID")]
    public int? MarketGroupId { get; init; }

    [JsonPropertyName("raceID")]
    public int? RaceId { get; init; }

    [JsonPropertyName("metaGroupID")]
    public int? MetaGroupId { get; init; }

    [JsonPropertyName("variationParentTypeID")]
    public int? VariationParentTypeId { get; init; }

    [JsonPropertyName("mass")]
    public double? Mass { get; init; }

    [JsonPropertyName("volume")]
    public double? Volume { get; init; }

    [JsonPropertyName("capacity")]
    public double? Capacity { get; init; }

    [JsonPropertyName("basePrice")]
    public decimal? BasePrice { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("portionSize")]
    public int PortionSize { get; init; }

    [JsonPropertyName("published")]
    public bool Published { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }
}
