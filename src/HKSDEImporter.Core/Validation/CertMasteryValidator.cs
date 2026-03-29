using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class CertMasteryValidator : IValidator<CertMastery>
{
    public ValidationResult Validate(CertMastery value)
    {
        if (value.TypeId <= 0) return ValidationResult.Invalid($"TypeId must be greater than 0, got {value.TypeId}.");
        if (value.CertId <= 0) return ValidationResult.Invalid($"CertId must be greater than 0, got {value.CertId}.");
        return ValidationResult.Valid();
    }
}
