using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class CategoryValidator : IValidator<Category>
{
    public ValidationResult Validate(Category value)
    {
        if (value.CategoryId <= 0)
        {
            return ValidationResult.Invalid($"CategoryId must be greater than 0, got {value.CategoryId}.");
        }

        if (string.IsNullOrWhiteSpace(value.Name))
        {
            return ValidationResult.Invalid($"Category {value.CategoryId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
