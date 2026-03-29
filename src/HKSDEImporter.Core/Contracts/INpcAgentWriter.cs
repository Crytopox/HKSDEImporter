using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface INpcAgentWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<Agent> agents,
        IReadOnlyCollection<ResearchAgent> researchAgents,
        IReadOnlyCollection<NpcCorporationResearchField> researchFields,
        IReadOnlyCollection<NpcCorporationTrade> corporationTrades,
        CancellationToken cancellationToken);
}
