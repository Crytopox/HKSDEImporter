namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawMastery
{
    public int TypeId { get; init; }
    public List<RawMasteryLevel>? Levels { get; init; }
}

public sealed class RawMasteryLevel
{
    public int MasteryLevel { get; init; }
    public List<int>? CertIds { get; init; }
}
