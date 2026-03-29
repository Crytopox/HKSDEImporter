namespace HKSDEImporter.Core.Models.Domain;

public sealed record Ancestry(int AncestryId, string AncestryName, int? BloodlineId, string? Description, int? Perception, int? Willpower, int? Charisma, int? Memory, int? Intelligence, int? IconId, string? ShortDescription);
