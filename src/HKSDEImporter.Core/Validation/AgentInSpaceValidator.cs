using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class AgentInSpaceValidator : IValidator<AgentInSpace>
{
    public ValidationResult Validate(AgentInSpace value)
    {
        return value.AgentId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"AgentId must be greater than 0, got {value.AgentId}.");
    }
}
