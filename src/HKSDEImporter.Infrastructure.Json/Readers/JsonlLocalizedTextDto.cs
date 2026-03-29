using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal sealed class JsonlLocalizedTextDto
{
    [JsonPropertyName("en")]
    public string? En { get; init; }
}
