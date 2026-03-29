using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlCategoryDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("published")]
    public bool Published { get; init; }
}
