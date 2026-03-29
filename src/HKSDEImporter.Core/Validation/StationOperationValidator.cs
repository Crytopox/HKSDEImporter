using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class StationOperationValidator : IValidator<StationOperation>
{
    public ValidationResult Validate(StationOperation value)
    {
        return value.OperationId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"OperationId must be greater than 0, got {value.OperationId}.");
    }
}
