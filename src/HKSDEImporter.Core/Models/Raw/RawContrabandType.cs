namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawContrabandType
{
    public int TypeId { get; init; }
    public List<RawContrabandFactionRule>? Factions { get; init; }
}

public sealed class RawContrabandFactionRule
{
    public int FactionId { get; init; }
    public double? StandingLoss { get; init; }
    public double? ConfiscateMinSec { get; init; }
    public double? FineByValue { get; init; }
    public double? AttackMinSec { get; init; }
}
