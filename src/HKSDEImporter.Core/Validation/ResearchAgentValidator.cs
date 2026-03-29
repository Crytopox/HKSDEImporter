using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class ResearchAgentValidator : IValidator<ResearchAgent>
{
    public ValidationResult Validate(ResearchAgent value)
    {
        if (value.AgentId <= 0) return ValidationResult.Invalid($"AgentId must be greater than 0, got {value.AgentId}.");
        return value.TypeId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"TypeId must be greater than 0, got {value.TypeId}.");
    }
}
