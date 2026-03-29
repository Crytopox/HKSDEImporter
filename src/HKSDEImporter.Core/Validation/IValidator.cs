using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public interface IValidator<in T>
{
    ValidationResult Validate(T value);
}
