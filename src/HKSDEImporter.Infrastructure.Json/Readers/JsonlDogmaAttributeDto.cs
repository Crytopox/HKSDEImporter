using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlDogmaAttributeDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("attributeCategoryID")]
    public int? AttributeCategoryId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("displayName")]
    public JsonlLocalizedTextDto? DisplayName { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("iconID")]
    public int? IconId { get; init; }

    [JsonPropertyName("defaultValue")]
    public double? DefaultValue { get; init; }

    [JsonPropertyName("published")]
    public bool Published { get; init; }

    [JsonPropertyName("unitID")]
    public int? UnitId { get; init; }

    [JsonPropertyName("stackable")]
    public bool Stackable { get; init; }

    [JsonPropertyName("highIsGood")]
    public bool HighIsGood { get; init; }
}
