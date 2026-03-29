namespace HKSDEImporter.Core.Models.Domain;

public sealed record DogmaEffect(
    int EffectId,
    string EffectName,
    int? EffectCategory,
    string? Description,
    string? Guid,
    int? IconId,
    bool IsOffensive,
    bool IsAssistance,
    int? DurationAttributeId,
    int? DischargeAttributeId,
    int? RangeAttributeId,
    bool DisallowAutoRepeat,
    bool Published,
    string? DisplayName,
    bool IsWarpSafe,
    bool RangeChance,
    bool ElectronicChance,
    bool PropulsionChance,
    int? Distribution,
    string? ModifierInfo);
