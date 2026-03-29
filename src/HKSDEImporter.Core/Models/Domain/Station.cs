namespace HKSDEImporter.Core.Models.Domain;

public sealed record Station(
    long StationId,
    double? Security,
    double? DockingCostPerVolume,
    double? MaxShipVolumeDockable,
    int? OfficeRentalCost,
    int? OperationId,
    int? StationTypeId,
    int? CorporationId,
    int? SolarSystemId,
    int? ConstellationId,
    int? RegionId,
    string? StationName,
    double? X,
    double? Y,
    double? Z,
    double? ReprocessingEfficiency,
    double? ReprocessingStationsTake,
    int? ReprocessingHangarFlag);
