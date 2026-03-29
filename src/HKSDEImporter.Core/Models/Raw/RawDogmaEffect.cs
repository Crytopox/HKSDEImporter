namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawDogmaEffect
{
    public int Key { get; init; }
    public string? Name { get; init; }
    public int? EffectCategoryId { get; init; }
    public RawLocalizedText? Description { get; init; }
    public string? Guid { get; init; }
    public int? IconId { get; init; }
    public bool IsOffensive { get; init; }
    public bool IsAssistance { get; init; }
    public int? DurationAttributeId { get; init; }
    public int? DischargeAttributeId { get; init; }
    public int? RangeAttributeId { get; init; }
    public bool DisallowAutoRepeat { get; init; }
    public bool Published { get; init; }
    public RawLocalizedText? DisplayName { get; init; }
    public bool IsWarpSafe { get; init; }
    public bool RangeChance { get; init; }
    public bool ElectronicChance { get; init; }
    public bool PropulsionChance { get; init; }
    public int? Distribution { get; init; }
    public string? ModifierInfoJson { get; init; }
}
