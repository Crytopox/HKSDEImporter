using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class FactionMapper : IMapper<RawFaction, Faction>
{
    public Faction Map(RawFaction raw)
    {
        return new Faction(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim(),
            raw.MemberRaces is { Count: > 0 } ? raw.MemberRaces[0] : null,
            raw.SolarSystemId,
            raw.CorporationId,
            raw.SizeFactor,
            null,
            null,
            raw.MilitiaCorporationId,
            raw.IconId);
    }
}
