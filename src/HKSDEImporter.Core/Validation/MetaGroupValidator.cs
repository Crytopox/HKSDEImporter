using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class MetaGroupValidator : IValidator<MetaGroup>
{
    public ValidationResult Validate(MetaGroup value)
    {
        if (value.MetaGroupId <= 0)
        {
            return ValidationResult.Invalid($"MetaGroupId must be greater than 0, got {value.MetaGroupId}.");
        }

        if (string.IsNullOrWhiteSpace(value.Name))
        {
            return ValidationResult.Invalid($"MetaGroup {value.MetaGroupId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
