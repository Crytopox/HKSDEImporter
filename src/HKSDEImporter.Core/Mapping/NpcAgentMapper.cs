using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class NpcAgentMapper
{
    public Agent? MapAgent(RawNpcCharacter raw)
    {
        if (raw.Agent is null)
        {
            return null;
        }

        return new Agent(
            raw.CharacterId,
            raw.Agent.DivisionId,
            raw.CorporationId,
            raw.LocationId,
            raw.Agent.Level,
            raw.Agent.Quality,
            raw.Agent.AgentTypeId,
            raw.Agent.IsLocator);
    }

    public IEnumerable<ResearchAgent> MapResearchAgents(RawNpcCharacter raw)
    {
        if (raw.Agent?.DivisionId != 18)
        {
            yield break;
        }

        foreach (var skillId in raw.SkillTypeIds ?? [])
        {
            yield return new ResearchAgent(raw.CharacterId, skillId);
        }
    }

    public IEnumerable<NpcCorporationResearchField> MapResearchFields(RawNpcCharacter raw)
    {
        if (raw.Agent?.DivisionId != 18 || !raw.CorporationId.HasValue)
        {
            yield break;
        }

        foreach (var skillId in raw.SkillTypeIds ?? [])
        {
            yield return new NpcCorporationResearchField(skillId, raw.CorporationId.Value);
        }
    }

    public IEnumerable<NpcCorporationTrade> MapCorporationTrades(RawNpcCorporation raw)
    {
        foreach (var trade in raw.CorporationTrades ?? [])
        {
            yield return new NpcCorporationTrade(raw.Key, trade.Key);
        }
    }
}
