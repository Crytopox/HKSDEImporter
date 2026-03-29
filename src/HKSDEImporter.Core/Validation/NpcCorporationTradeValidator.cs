using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class NpcCorporationTradeValidator : IValidator<NpcCorporationTrade>
{
    public ValidationResult Validate(NpcCorporationTrade value)
    {
        if (value.CorporationId <= 0) return ValidationResult.Invalid($"CorporationId must be greater than 0, got {value.CorporationId}.");
        return value.TypeId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"TypeId must be greater than 0, got {value.TypeId}.");
    }
}
