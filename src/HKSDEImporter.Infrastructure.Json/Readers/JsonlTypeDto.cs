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

    [JsonPropertyName("mass")]
    public double? Mass { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("portionSize")]
    public int PortionSize { get; init; }

    [JsonPropertyName("published")]
    public bool Published { get; init; }

    [JsonPropertyName("radius")]
    public double? Radius { get; init; }

    [JsonPropertyName("volume")]
    public double? Volume { get; init; }
}
