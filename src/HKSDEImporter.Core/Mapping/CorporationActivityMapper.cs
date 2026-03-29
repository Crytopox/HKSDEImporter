using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class CorporationActivityMapper : IMapper<RawCorporationActivity, CorporationActivity>
{
    public CorporationActivity Map(RawCorporationActivity raw)
    {
        return new CorporationActivity(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            null);
    }
}
