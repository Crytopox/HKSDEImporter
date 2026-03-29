using System.Text.Json;
using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlBlueprintDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("blueprintTypeID")]
    public int? BlueprintTypeId { get; init; }

    [JsonPropertyName("maxProductionLimit")]
    public int MaxProductionLimit { get; init; }

    [JsonPropertyName("activities")]
    public Dictionary<string, JsonElement>? Activities { get; init; }
}

internal sealed class JsonlPlanetSchematicsDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("cycleTime")]
    public int CycleTime { get; init; }

    [JsonPropertyName("pins")]
    public List<int>? Pins { get; init; }

    [JsonPropertyName("types")]
    public List<JsonlPlanetSchematicTypeDto>? Types { get; init; }
}

internal sealed class JsonlPlanetSchematicTypeDto
{
    [JsonPropertyName("_key")]
    public int TypeId { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }

    [JsonPropertyName("isInput")]
    public bool IsInput { get; init; }
}
