using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class AgentInSpaceMapper : IMapper<RawAgentInSpace, AgentInSpace>
{
    public AgentInSpace Map(RawAgentInSpace raw)
    {
        return new AgentInSpace(raw.AgentId, raw.DungeonId, raw.SolarSystemId, raw.SpawnPointId, raw.TypeId);
    }
}
