using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class AncestryMapper : IMapper<RawAncestry, Ancestry>
{
    public Ancestry Map(RawAncestry raw)
    {
        return new Ancestry(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.BloodlineId,
            raw.Description?.En?.Trim(),
            raw.Perception,
            raw.Willpower,
            raw.Charisma,
            raw.Memory,
            raw.Intelligence,
            raw.IconId,
            raw.ShortDescription?.En?.Trim());
    }
}
