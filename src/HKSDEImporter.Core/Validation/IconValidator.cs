using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class IconValidator : IValidator<Icon>
{
    public ValidationResult Validate(Icon value)
    {
        return value.IconId >= 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"IconId must be >= 0, got {value.IconId}.");
    }
}
