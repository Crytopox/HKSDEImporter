namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawStationOperation
{
    public int Key { get; init; }
    public int? ActivityId { get; init; }
    public RawLocalizedText? OperationName { get; init; }
    public RawLocalizedText? Description { get; init; }
    public double? Fringe { get; init; }
    public double? Corridor { get; init; }
    public double? Hub { get; init; }
    public double? Border { get; init; }
    public double? Ratio { get; init; }
    public List<int>? Services { get; init; }
    public List<RawRaceStationType>? StationTypes { get; init; }
}

public sealed class RawRaceStationType
{
    public int RaceKey { get; init; }
    public int StationTypeId { get; init; }
}
