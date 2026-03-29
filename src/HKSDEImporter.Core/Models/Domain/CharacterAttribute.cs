namespace HKSDEImporter.Core.Models.Domain;

public sealed record CharacterAttribute(int AttributeId, string AttributeName, string? Description, int? IconId, string? ShortDescription, string? Notes);
