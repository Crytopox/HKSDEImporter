using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class AncestryValidator : IValidator<Ancestry>
{
    public ValidationResult Validate(Ancestry value)
    {
        if (value.AncestryId <= 0)
        {
            return ValidationResult.Invalid($"AncestryId must be greater than 0, got {value.AncestryId}.");
        }

        if (string.IsNullOrWhiteSpace(value.AncestryName))
        {
            return ValidationResult.Invalid($"Ancestry {value.AncestryId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
