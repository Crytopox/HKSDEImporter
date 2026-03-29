using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class CertValidator : IValidator<Cert>
{
    public ValidationResult Validate(Cert value)
        => value.CertId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"CertId must be greater than 0, got {value.CertId}.");
}
