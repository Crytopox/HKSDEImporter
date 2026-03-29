using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class MapConstellationValidator : IValidator<MapConstellation>
{
    public ValidationResult Validate(MapConstellation value)
    {
        if (value.ConstellationId <= 0)
        {
            return ValidationResult.Invalid($"ConstellationId must be greater than 0, got {value.ConstellationId}.");
        }

        if (string.IsNullOrWhiteSpace(value.ConstellationName))
        {
            return ValidationResult.Invalid($"Constellation {value.ConstellationId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
