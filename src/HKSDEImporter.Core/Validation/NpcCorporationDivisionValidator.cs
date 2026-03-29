using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class NpcCorporationDivisionValidator : IValidator<NpcCorporationDivision>
{
    public ValidationResult Validate(NpcCorporationDivision value)
    {
        if (value.DivisionId <= 0)
        {
            return ValidationResult.Invalid($"DivisionId must be greater than 0, got {value.DivisionId}.");
        }

        if (string.IsNullOrWhiteSpace(value.DivisionName))
        {
            return ValidationResult.Invalid($"NPC division {value.DivisionId} has an empty English name.");
        }

        return ValidationResult.Valid();
    }
}
