using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlNpcCharacterDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("corporationID")]
    public int? CorporationId { get; init; }

    [JsonPropertyName("locationID")]
    public int? LocationId { get; init; }

    [JsonPropertyName("agent")]
    public JsonlNpcAgentDto? Agent { get; init; }

    [JsonPropertyName("skills")]
    public List<JsonlTypeIdOnlyDto>? Skills { get; init; }
}

internal sealed class JsonlNpcAgentDto
{
    [JsonPropertyName("agentTypeID")]
    public int? AgentTypeId { get; init; }

    [JsonPropertyName("divisionID")]
    public int? DivisionId { get; init; }

    [JsonPropertyName("isLocator")]
    public bool? IsLocator { get; init; }

    [JsonPropertyName("level")]
    public int? Level { get; init; }

    [JsonPropertyName("quality")]
    public int? Quality { get; init; }
}

internal sealed class JsonlTypeIdOnlyDto
{
    [JsonPropertyName("typeID")]
    public int? TypeId { get; init; }
}

internal sealed class JsonlIdDoubleDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("_value")]
    public double? Value { get; init; }
}
