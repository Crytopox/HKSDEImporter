namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawNpcStation
{
    public long Key { get; init; }
    public int? OperationId { get; init; }
    public int? OwnerId { get; init; }
    public int? SolarSystemId { get; init; }
    public int? TypeId { get; init; }
    public RawPosition3D? Position { get; init; }
    public double? ReprocessingEfficiency { get; init; }
    public double? ReprocessingStationsTake { get; init; }
    public int? ReprocessingHangarFlag { get; init; }
}

public sealed class RawPosition3D
{
    public double? X { get; init; }
    public double? Y { get; init; }
    public double? Z { get; init; }
}
