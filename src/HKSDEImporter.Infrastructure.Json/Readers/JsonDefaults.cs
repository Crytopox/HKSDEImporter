using System.Text.Json;

namespace HKSDEImporter.Infrastructure.Json.Readers;

internal static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
