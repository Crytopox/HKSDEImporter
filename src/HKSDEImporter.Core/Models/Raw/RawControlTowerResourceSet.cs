namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawControlTowerResourceSet
{
    public int ControlTowerTypeId { get; init; }
    public List<RawControlTowerResource>? Resources { get; init; }
}

public sealed class RawControlTowerResource
{
    public int ResourceTypeId { get; init; }
    public int? Purpose { get; init; }
    public int? Quantity { get; init; }
    public double? MinSecurityLevel { get; init; }
    public int? FactionId { get; init; }
}
