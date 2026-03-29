using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class TypeValidator : IValidator<TypeItem>
{
    public ValidationResult Validate(TypeItem value)
    {
        if (value.TypeId <= 0)
        {
            return ValidationResult.Invalid($"TypeId must be greater than 0, got {value.TypeId}.");
        }

        if (value.GroupId <= 0)
        {
            return ValidationResult.Invalid($"Type {value.TypeId} has invalid GroupId {value.GroupId}.");
        }

        if (string.IsNullOrWhiteSpace(value.Name))
        {
            return ValidationResult.Invalid($"Type {value.TypeId} has an empty English name.");
        }

        if (value.PortionSize <= 0)
        {
            return ValidationResult.Invalid($"Type {value.TypeId} has invalid portion size {value.PortionSize}.");
        }

        return ValidationResult.Valid();
    }
}
