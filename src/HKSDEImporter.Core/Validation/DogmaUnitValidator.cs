using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class DogmaUnitValidator : IValidator<DogmaUnit>
{
    public ValidationResult Validate(DogmaUnit value)
    {
        if (value.UnitId <= 0)
        {
            return ValidationResult.Invalid($"UnitId must be greater than 0, got {value.UnitId}.");
        }

        if (string.IsNullOrWhiteSpace(value.UnitName))
        {
            return ValidationResult.Invalid($"Dogma unit {value.UnitId} has an empty name.");
        }

        return ValidationResult.Valid();
    }
}
