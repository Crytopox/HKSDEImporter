using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class NpcCorporationDivisionMapper : IMapper<RawNpcCorporationDivision, NpcCorporationDivision>
{
    public NpcCorporationDivision Map(RawNpcCorporationDivision raw)
    {
        return new NpcCorporationDivision(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.DisplayName?.Trim(),
            raw.LeaderTypeName?.En?.Trim());
    }
}
