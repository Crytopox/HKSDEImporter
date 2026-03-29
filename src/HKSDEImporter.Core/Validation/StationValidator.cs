using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class StationValidator : IValidator<Station>
{
    public ValidationResult Validate(Station value)
    {
        return value.StationId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"StationId must be greater than 0, got {value.StationId}.");
    }
}
