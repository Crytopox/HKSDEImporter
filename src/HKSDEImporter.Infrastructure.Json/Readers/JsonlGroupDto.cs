using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlGroupDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("categoryID")]
    public int CategoryId { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("useBasePrice")]
    public bool UseBasePrice { get; init; }

    [JsonPropertyName("anchored")]
    public bool Anchored { get; init; }

    [JsonPropertyName("anchorable")]
    public bool Anchorable { get; init; }

    [JsonPropertyName("fittableNonSingleton")]
    public bool FittableNonSingleton { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("published")]
    public bool Published { get; init; }
}
