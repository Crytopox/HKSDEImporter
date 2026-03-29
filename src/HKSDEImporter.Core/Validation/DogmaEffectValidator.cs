using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class DogmaEffectValidator : IValidator<DogmaEffect>
{
    public ValidationResult Validate(DogmaEffect value)
    {
        if (value.EffectId <= 0)
        {
            return ValidationResult.Invalid($"Dogma effect id must be > 0, got {value.EffectId}.");
        }

        if (string.IsNullOrWhiteSpace(value.EffectName))
        {
            return ValidationResult.Invalid($"Dogma effect {value.EffectId} has empty name.");
        }

        return ValidationResult.Valid();
    }
}
