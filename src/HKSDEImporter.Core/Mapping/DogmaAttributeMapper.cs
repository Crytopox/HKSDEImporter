using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class DogmaAttributeMapper : IMapper<RawDogmaAttribute, DogmaAttributeType>
{
    public DogmaAttributeType Map(RawDogmaAttribute raw)
    {
        return new DogmaAttributeType(
            raw.Key,
            raw.Name?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim(),
            raw.IconId,
            raw.DefaultValue,
            raw.Published,
            raw.DisplayName?.En?.Trim(),
            raw.UnitId,
            raw.Stackable,
            raw.HighIsGood,
            raw.AttributeCategoryId);
    }
}
