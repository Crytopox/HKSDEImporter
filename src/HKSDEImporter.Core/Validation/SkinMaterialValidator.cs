using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class SkinMaterialValidator : IValidator<SkinMaterial>
{
    public ValidationResult Validate(SkinMaterial value)
        => value.SkinMaterialId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"SkinMaterialId must be greater than 0, got {value.SkinMaterialId}.");
}
