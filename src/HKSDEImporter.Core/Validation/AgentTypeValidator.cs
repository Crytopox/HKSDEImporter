using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class AgentTypeValidator : IValidator<AgentType>
{
    public ValidationResult Validate(AgentType value)
    {
        return value.AgentTypeId > 0
            ? ValidationResult.Valid()
            : ValidationResult.Invalid($"AgentTypeId must be greater than 0, got {value.AgentTypeId}.");
    }
}
