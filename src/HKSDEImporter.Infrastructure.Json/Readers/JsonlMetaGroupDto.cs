using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlMetaGroupDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }
}
