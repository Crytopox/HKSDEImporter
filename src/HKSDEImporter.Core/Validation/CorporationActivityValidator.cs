using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class CorporationActivityValidator : IValidator<CorporationActivity>
{
    public ValidationResult Validate(CorporationActivity value)
    {
        if (value.ActivityId <= 0)
        {
            return ValidationResult.Invalid($"ActivityId must be greater than 0, got {value.ActivityId}.");
        }

        if (string.IsNullOrWhiteSpace(value.ActivityName))
        {
            return ValidationResult.Invalid($"Corporation activity {value.ActivityId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
