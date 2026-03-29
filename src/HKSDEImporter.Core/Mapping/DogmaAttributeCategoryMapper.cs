using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class DogmaAttributeCategoryMapper : IMapper<RawDogmaAttributeCategory, DogmaAttributeCategory>
{
    public DogmaAttributeCategory Map(RawDogmaAttributeCategory raw)
    {
        return new DogmaAttributeCategory(
            raw.Key,
            raw.Name?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim());
    }
}
