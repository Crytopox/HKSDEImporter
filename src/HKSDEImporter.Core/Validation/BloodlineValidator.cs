using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class BloodlineValidator : IValidator<Bloodline>
{
    public ValidationResult Validate(Bloodline value)
    {
        if (value.BloodlineId <= 0)
        {
            return ValidationResult.Invalid($"BloodlineId must be greater than 0, got {value.BloodlineId}.");
        }

        if (string.IsNullOrWhiteSpace(value.BloodlineName))
        {
            return ValidationResult.Invalid($"Bloodline {value.BloodlineId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
