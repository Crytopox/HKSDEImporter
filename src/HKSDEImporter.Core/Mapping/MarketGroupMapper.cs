using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class MarketGroupMapper : IMapper<RawMarketGroup, MarketGroup>
{
    public MarketGroup Map(RawMarketGroup raw)
    {
        return new MarketGroup(
            raw.Key,
            raw.ParentGroupId,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim(),
            raw.IconId,
            raw.HasTypes);
    }
}
