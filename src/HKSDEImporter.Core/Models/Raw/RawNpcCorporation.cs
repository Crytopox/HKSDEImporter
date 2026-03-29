namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawNpcCorporation
{
    public int Key { get; init; }
    public RawLocalizedText? Name { get; init; }
    public RawLocalizedText? Description { get; init; }
    public string? Size { get; init; }
    public string? Extent { get; init; }
    public int? SolarSystemId { get; init; }
    public List<RawNpcCorporationInvestor>? Investors { get; init; }
    public int? FriendId { get; init; }
    public int? EnemyId { get; init; }
    public long? Shares { get; init; }
    public int? InitialPrice { get; init; }
    public double? MinSecurity { get; init; }
    public int? FactionId { get; init; }
    public double? SizeFactor { get; init; }
    public int? IconId { get; init; }
    public List<RawNpcCorporationTrade>? CorporationTrades { get; init; }
    public List<RawNpcCorporationDivisionLink>? Divisions { get; init; }
}

public sealed class RawNpcCorporationInvestor
{
    public int Key { get; init; }
    public int Value { get; init; }
}

public sealed class RawNpcCorporationTrade
{
    public int Key { get; init; }
    public double? Value { get; init; }
}

public sealed class RawNpcCorporationDivisionLink
{
    public int DivisionId { get; init; }
    public int? Size { get; init; }
}
