using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class ContrabandTypeRuleValidator : IValidator<ContrabandTypeRule>
{
    public ValidationResult Validate(ContrabandTypeRule value)
    {
        if (value.FactionId <= 0)
        {
            return ValidationResult.Invalid($"Contraband faction id must be greater than 0, got {value.FactionId}.");
        }

        return value.TypeId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"Contraband type id must be greater than 0, got {value.TypeId}.");
    }
}
