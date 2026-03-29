using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class SkinLicenseValidator : IValidator<SkinLicense>
{
    public ValidationResult Validate(SkinLicense value)
        => value.LicenseTypeId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"LicenseTypeId must be greater than 0, got {value.LicenseTypeId}.");
}
