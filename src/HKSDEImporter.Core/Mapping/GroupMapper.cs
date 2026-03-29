using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class GroupMapper : IMapper<RawGroup, Group>
{
    public Group Map(RawGroup raw)
    {
        return new Group(
            raw.Key,
            raw.CategoryId,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Published);
    }
}
