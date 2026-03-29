namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawDogmaAttribute
{
    public int Key { get; init; }
    public int? AttributeCategoryId { get; init; }
    public string? Name { get; init; }
    public RawLocalizedText? DisplayName { get; init; }
    public RawLocalizedText? Description { get; init; }
    public int? IconId { get; init; }
    public double? DefaultValue { get; init; }
    public bool Published { get; init; }
    public int? UnitId { get; init; }
    public bool Stackable { get; init; }
    public bool HighIsGood { get; init; }
}
