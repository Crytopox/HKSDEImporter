using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class ControlTowerResourceValidator : IValidator<ControlTowerResource>
{
    public ValidationResult Validate(ControlTowerResource value)
    {
        if (value.ControlTowerTypeId <= 0)
        {
            return ValidationResult.Invalid($"Control tower type id must be greater than 0, got {value.ControlTowerTypeId}.");
        }

        return value.ResourceTypeId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"Resource type id must be greater than 0, got {value.ResourceTypeId}.");
    }
}
