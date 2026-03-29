namespace HKSDEImporter.Core.Models.Domain;

public sealed record ControlTowerResource(
    int ControlTowerTypeId,
    int ResourceTypeId,
    int? Purpose,
    int? Quantity,
    double? MinSecurityLevel,
    int? FactionId);
