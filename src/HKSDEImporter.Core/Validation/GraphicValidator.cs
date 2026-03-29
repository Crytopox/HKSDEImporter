using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class GraphicValidator : IValidator<Graphic>
{
    public ValidationResult Validate(Graphic value)
    {
        return value.GraphicId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"GraphicId must be greater than 0, got {value.GraphicId}.");
    }
}
