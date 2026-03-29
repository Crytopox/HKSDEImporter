using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class TranslationLanguageMapper : IMapper<RawTranslationLanguage, TranslationLanguage>
{
    private static readonly IReadOnlyDictionary<string, int> LanguageToLcid = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = 1033,
        ["es"] = 1034,
        ["fr"] = 1036,
        ["ja"] = 1041,
        ["ko"] = 1042,
        ["ru"] = 1049,
        ["de"] = 1050,
        ["zh"] = 1051
    };

    public TranslationLanguage Map(RawTranslationLanguage raw)
    {
        var languageId = string.Equals(raw.Key, "en", StringComparison.OrdinalIgnoreCase)
            ? "en-us"
            : raw.Key.ToLowerInvariant();

        var numericId = LanguageToLcid.TryGetValue(raw.Key, out var lcid) ? lcid : 0;

        return new TranslationLanguage(numericId, languageId, raw.Name?.Trim());
    }
}
