using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlCertificateDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("name")]
    public JsonlLocalizedTextDto? Name { get; init; }

    [JsonPropertyName("description")]
    public JsonlLocalizedTextDto? Description { get; init; }

    [JsonPropertyName("groupID")]
    public int? GroupId { get; init; }

    [JsonPropertyName("skillTypes")]
    public List<JsonlCertificateSkillDto>? SkillTypes { get; init; }
}

internal sealed class JsonlCertificateSkillDto
{
    [JsonPropertyName("_key")]
    public int SkillId { get; init; }

    [JsonPropertyName("basic")]
    public int? Basic { get; init; }

    [JsonPropertyName("standard")]
    public int? Standard { get; init; }

    [JsonPropertyName("improved")]
    public int? Improved { get; init; }

    [JsonPropertyName("advanced")]
    public int? Advanced { get; init; }

    [JsonPropertyName("elite")]
    public int? Elite { get; init; }
}

internal sealed class JsonlMasteryDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("_value")]
    public List<JsonlMasteryLevelDto>? Levels { get; init; }
}

internal sealed class JsonlMasteryLevelDto
{
    [JsonPropertyName("_key")]
    public int MasteryLevel { get; init; }

    [JsonPropertyName("_value")]
    public List<int>? CertIds { get; init; }
}

internal sealed class JsonlSkinDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("internalName")]
    public string? InternalName { get; init; }

    [JsonPropertyName("skinMaterialID")]
    public int? SkinMaterialId { get; init; }

    [JsonPropertyName("types")]
    public List<int>? Types { get; init; }
}

internal sealed class JsonlSkinMaterialDto
{
    [JsonPropertyName("_key")]
    public int Key { get; init; }

    [JsonPropertyName("displayName")]
    public JsonlLocalizedTextDto? DisplayName { get; init; }

    [JsonPropertyName("materialSetID")]
    public int? MaterialSetId { get; init; }
}

internal sealed class JsonlSkinLicenseDto
{
    [JsonPropertyName("licenseTypeID")]
    public int LicenseTypeId { get; init; }

    [JsonPropertyName("duration")]
    public int? Duration { get; init; }

    [JsonPropertyName("skinID")]
    public int? SkinId { get; init; }
}
