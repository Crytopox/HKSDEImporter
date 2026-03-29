using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class MapRegionValidator : IValidator<MapRegion>
{
    public ValidationResult Validate(MapRegion value)
    {
        if (value.RegionId <= 0)
        {
            return ValidationResult.Invalid($"RegionId must be greater than 0, got {value.RegionId}.");
        }

        if (string.IsNullOrWhiteSpace(value.RegionName))
        {
            return ValidationResult.Invalid($"Region {value.RegionId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
