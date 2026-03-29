namespace HKSDEImporter.Core.Models.Domain;

public sealed record Agent(
    int AgentId,
    int? DivisionId,
    int? CorporationId,
    int? LocationId,
    int? Level,
    int? Quality,
    int? AgentTypeId,
    bool? IsLocator);
