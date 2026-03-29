using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class SkinValidator : IValidator<Skin>
{
    public ValidationResult Validate(Skin value)
        => value.SkinId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"SkinId must be greater than 0, got {value.SkinId}.");
}
