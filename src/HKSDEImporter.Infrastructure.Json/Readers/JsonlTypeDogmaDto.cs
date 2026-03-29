using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlTypeDogmaDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("dogmaAttributes")]
    public List<JsonlTypeDogmaAttributeDto>? DogmaAttributes { get; init; }

    [JsonPropertyName("dogmaEffects")]
    public List<JsonlTypeDogmaEffectDto>? DogmaEffects { get; init; }
}

internal sealed class JsonlTypeDogmaAttributeDto
{
    [JsonPropertyName("attributeID")]
    public int AttributeId { get; init; }

    [JsonPropertyName("value")]
    public double Value { get; init; }
}

internal sealed class JsonlTypeDogmaEffectDto
{
    [JsonPropertyName("effectID")]
    public int EffectId { get; init; }

    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; init; }
}
