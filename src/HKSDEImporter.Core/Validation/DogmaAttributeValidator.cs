using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class DogmaAttributeValidator : IValidator<DogmaAttributeType>
{
    public ValidationResult Validate(DogmaAttributeType value)
    {
        if (value.AttributeId <= 0)
        {
            return ValidationResult.Invalid($"Dogma attribute id must be > 0, got {value.AttributeId}.");
        }

        if (string.IsNullOrWhiteSpace(value.AttributeName))
        {
            return ValidationResult.Invalid($"Dogma attribute {value.AttributeId} has empty name.");
        }

        return ValidationResult.Valid();
    }
}
