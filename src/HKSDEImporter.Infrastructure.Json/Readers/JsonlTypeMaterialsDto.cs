using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlTypeMaterialsDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("materials")]
    public List<JsonlTypeMaterialDto>? Materials { get; init; }
}

internal sealed class JsonlTypeMaterialDto
{
    [JsonPropertyName("materialTypeID")]
    public int MaterialTypeId { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }
}
