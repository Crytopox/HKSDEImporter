using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class NpcCorporationValidator : IValidator<NpcCorporation>
{
    public ValidationResult Validate(NpcCorporation value)
    {
        return value.CorporationId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"CorporationId must be greater than 0, got {value.CorporationId}.");
    }
}
