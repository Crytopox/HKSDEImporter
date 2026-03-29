using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class BloodlineMapper : IMapper<RawBloodline, Bloodline>
{
    public Bloodline Map(RawBloodline raw)
    {
        return new Bloodline(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.RaceId,
            raw.Description?.En?.Trim(),
            raw.MaleDescription?.En?.Trim(),
            raw.FemaleDescription?.En?.Trim(),
            raw.ShipTypeId,
            raw.CorporationId,
            raw.Perception,
            raw.Willpower,
            raw.Charisma,
            raw.Memory,
            raw.Intelligence,
            raw.IconId,
            raw.ShortDescription?.En?.Trim(),
            raw.ShortMaleDescription?.En?.Trim(),
            raw.ShortFemaleDescription?.En?.Trim());
    }
}
