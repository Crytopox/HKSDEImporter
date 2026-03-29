using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class MetaGroupMapper : IMapper<RawMetaGroup, MetaGroup>
{
    public MetaGroup Map(RawMetaGroup raw)
    {
        return new MetaGroup(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim(),
            raw.IconId);
    }
}
