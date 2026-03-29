using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class DogmaAttributeCategoryValidator : IValidator<DogmaAttributeCategory>
{
    public ValidationResult Validate(DogmaAttributeCategory value)
    {
        if (value.CategoryId <= 0)
        {
            return ValidationResult.Invalid($"Dogma attribute category id must be > 0, got {value.CategoryId}.");
        }

        if (string.IsNullOrWhiteSpace(value.CategoryName))
        {
            return ValidationResult.Invalid($"Dogma attribute category {value.CategoryId} has empty name.");
        }

        return ValidationResult.Valid();
    }
}
