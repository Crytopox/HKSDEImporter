using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class CertSkillValidator : IValidator<CertSkill>
{
    public ValidationResult Validate(CertSkill value)
    {
        if (value.CertId <= 0) return ValidationResult.Invalid($"CertId must be greater than 0, got {value.CertId}.");
        if (value.SkillId <= 0) return ValidationResult.Invalid($"SkillId must be greater than 0, got {value.SkillId}.");
        return ValidationResult.Valid();
    }
}
