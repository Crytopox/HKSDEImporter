using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class CategoryMapper : IMapper<RawCategory, Category>
{
    public Category Map(RawCategory raw)
    {
        return new Category(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Published,
            raw.IconId);
    }
}
