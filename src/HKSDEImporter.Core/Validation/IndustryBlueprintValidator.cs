using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class IndustryBlueprintValidator : IValidator<IndustryBlueprint>
{
    public ValidationResult Validate(IndustryBlueprint value)
    {
        if (value.TypeId <= 0)
        {
            return ValidationResult.Invalid($"Blueprint type id must be > 0, got {value.TypeId}.");
        }

        if (value.MaxProductionLimit < 0)
        {
            return ValidationResult.Invalid($"Blueprint {value.TypeId} has invalid max production limit {value.MaxProductionLimit}.");
        }

        return ValidationResult.Valid();
    }
}
