using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class CharacterAttributeMapper : IMapper<RawCharacterAttribute, CharacterAttribute>
{
    public CharacterAttribute Map(RawCharacterAttribute raw)
    {
        return new CharacterAttribute(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim(),
            raw.IconId,
            raw.ShortDescription?.En?.Trim(),
            raw.Notes?.Trim());
    }
}
