using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class GroupValidator : IValidator<Group>
{
    public ValidationResult Validate(Group value)
    {
        if (value.GroupId <= 0)
        {
            return ValidationResult.Invalid($"GroupId must be greater than 0, got {value.GroupId}.");
        }

        if (value.CategoryId <= 0)
        {
            return ValidationResult.Invalid($"Group {value.GroupId} has invalid CategoryId {value.CategoryId}.");
        }

        if (string.IsNullOrWhiteSpace(value.Name))
        {
            return ValidationResult.Invalid($"Group {value.GroupId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
