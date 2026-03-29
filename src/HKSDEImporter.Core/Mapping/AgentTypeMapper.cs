using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class AgentTypeMapper : IMapper<RawAgentType, AgentType>
{
    public AgentType Map(RawAgentType raw)
    {
        return new AgentType(raw.Key, raw.Name?.Trim());
    }
}
