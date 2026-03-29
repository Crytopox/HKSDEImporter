using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class DogmaUnitMapper : IMapper<RawDogmaUnit, DogmaUnit>
{
    public DogmaUnit Map(RawDogmaUnit raw)
    {
        return new DogmaUnit(
            raw.Key,
            raw.Name?.Trim() ?? string.Empty,
            raw.DisplayName?.En?.Trim(),
            raw.Description?.En?.Trim());
    }
}
