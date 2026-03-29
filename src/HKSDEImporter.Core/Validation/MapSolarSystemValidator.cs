using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class MapSolarSystemValidator : IValidator<MapSolarSystem>
{
    public ValidationResult Validate(MapSolarSystem value)
    {
        if (value.SolarSystemId <= 0)
        {
            return ValidationResult.Invalid($"SolarSystemId must be greater than 0, got {value.SolarSystemId}.");
        }

        if (string.IsNullOrWhiteSpace(value.SolarSystemName))
        {
            return ValidationResult.Invalid($"Solar system {value.SolarSystemId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
