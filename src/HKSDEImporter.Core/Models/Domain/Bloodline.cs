namespace HKSDEImporter.Core.Models.Domain;

public sealed record Bloodline(int BloodlineId, string BloodlineName, int? RaceId, string? Description, string? MaleDescription, string? FemaleDescription, int? ShipTypeId, int? CorporationId, int? Perception, int? Willpower, int? Charisma, int? Memory, int? Intelligence, int? IconId, string? ShortDescription, string? ShortMaleDescription, string? ShortFemaleDescription);
