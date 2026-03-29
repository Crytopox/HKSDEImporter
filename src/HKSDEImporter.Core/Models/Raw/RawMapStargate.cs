namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMapStargate
{
    public int StargateId { get; init; }
    public int? SolarSystemId { get; init; }
    public int? DestinationSolarSystemId { get; init; }
    public int? DestinationStargateId { get; init; }
}
