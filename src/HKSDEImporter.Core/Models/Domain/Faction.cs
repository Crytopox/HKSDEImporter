namespace HKSDEImporter.Core.Models.Domain;

public sealed record Faction(int FactionId, string FactionName, string? Description, int? RaceIds, int? SolarSystemId, int? CorporationId, double? SizeFactor, int? StationCount, int? StationSystemCount, int? MilitiaCorporationId, int? IconId);
