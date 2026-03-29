using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class MarketGroupValidator : IValidator<MarketGroup>
{
    public ValidationResult Validate(MarketGroup value)
    {
        if (value.MarketGroupId <= 0)
        {
            return ValidationResult.Invalid($"MarketGroupId must be greater than 0, got {value.MarketGroupId}.");
        }

        if (string.IsNullOrWhiteSpace(value.Name))
        {
            return ValidationResult.Invalid($"MarketGroup {value.MarketGroupId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
