using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class FactionValidator : IValidator<Faction>
{
    public ValidationResult Validate(Faction value)
    {
        if (value.FactionId <= 0)
        {
            return ValidationResult.Invalid($"FactionId must be greater than 0, got {value.FactionId}.");
        }

        if (string.IsNullOrWhiteSpace(value.FactionName))
        {
            return ValidationResult.Invalid($"Faction {value.FactionId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
