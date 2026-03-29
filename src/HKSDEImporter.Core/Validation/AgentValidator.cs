using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Validation;

public sealed class AgentValidator : IValidator<Agent>
{
    public ValidationResult Validate(Agent value)
        => value.AgentId > 0 ? ValidationResult.Valid() : ValidationResult.Invalid($"AgentId must be greater than 0, got {value.AgentId}.");
}
