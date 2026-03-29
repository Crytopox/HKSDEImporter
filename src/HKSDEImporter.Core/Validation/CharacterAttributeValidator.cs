using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class CharacterAttributeValidator : IValidator<CharacterAttribute>
{
    public ValidationResult Validate(CharacterAttribute value)
    {
        if (value.AttributeId <= 0)
        {
            return ValidationResult.Invalid($"AttributeId must be greater than 0, got {value.AttributeId}.");
        }

        if (string.IsNullOrWhiteSpace(value.AttributeName))
        {
            return ValidationResult.Invalid($"Character attribute {value.AttributeId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
