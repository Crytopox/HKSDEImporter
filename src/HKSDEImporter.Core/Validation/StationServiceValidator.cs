using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class StationServiceValidator : IValidator<StationService>
{
    public ValidationResult Validate(StationService value)
    {
        return value.ServiceId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"ServiceId must be greater than 0, got {value.ServiceId}.");
    }
}
