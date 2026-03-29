using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IAgentReferenceWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<AgentType> agentTypes, IReadOnlyCollection<AgentInSpace> agentsInSpace, CancellationToken cancellationToken);
}
