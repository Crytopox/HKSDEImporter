using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlMarketGroupDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("parentGroupID")]
    public int? ParentGroupId { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("hasTypes")]
    public bool HasTypes { get; init; }
}
