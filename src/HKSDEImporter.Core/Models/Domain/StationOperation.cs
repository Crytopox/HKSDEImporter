namespace HKSDEImporter.Core.Models.Domain;

public sealed record StationOperation(
    int OperationId,
    int? ActivityId,
    string? OperationName,
    string? Description,
    double? Fringe,
    double? Corridor,
    double? Hub,
    double? Border,
    double? Ratio,
    int? CaldariStationTypeId,
    int? MinmatarStationTypeId,
    int? AmarrStationTypeId,
    int? GallenteStationTypeId,
    int? JoveStationTypeId);
