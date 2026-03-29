using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class SkinShipValidator : IValidator<SkinShip>
{
    public ValidationResult Validate(SkinShip value)
    {
        if (value.SkinId <= 0) return ValidationResult.Invalid($"SkinId must be greater than 0, got {value.SkinId}.");
        return value.TypeId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"TypeId must be greater than 0, got {value.TypeId}.");
    }
}
