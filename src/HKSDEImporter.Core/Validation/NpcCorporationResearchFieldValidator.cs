using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class NpcCorporationResearchFieldValidator : IValidator<NpcCorporationResearchField>
{
    public ValidationResult Validate(NpcCorporationResearchField value)
    {
        if (value.SkillId <= 0) return ValidationResult.Invalid($"SkillId must be greater than 0, got {value.SkillId}.");
        return value.CorporationId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"CorporationId must be greater than 0, got {value.CorporationId}.");
    }
}
