namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawNpcCharacter
{
    public int CharacterId { get; init; }
    public int? CorporationId { get; init; }
    public int? LocationId { get; init; }
    public RawNpcAgentData? Agent { get; init; }
    public List<int>? SkillTypeIds { get; init; }
}

public sealed class RawNpcAgentData
{
    public int? AgentTypeId { get; init; }
    public int? DivisionId { get; init; }
    public bool? IsLocator { get; init; }
    public int? Level { get; init; }
    public int? Quality { get; init; }
}
