using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class RaceValidator : IValidator<Race>
{
    public ValidationResult Validate(Race value)
    {
        if (value.RaceId <= 0)
        {
            return ValidationResult.Invalid($"RaceId must be greater than 0, got {value.RaceId}.");
        }

        if (string.IsNullOrWhiteSpace(value.RaceName))
        {
            return ValidationResult.Invalid($"Race {value.RaceId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
