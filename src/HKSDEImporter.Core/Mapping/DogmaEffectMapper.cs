using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class DogmaEffectMapper : IMapper<RawDogmaEffect, DogmaEffect>
{
    public DogmaEffect Map(RawDogmaEffect raw)
    {
        return new DogmaEffect(
            raw.Key,
            raw.Name?.Trim() ?? string.Empty,
            raw.EffectCategoryId,
            raw.Description?.En?.Trim(),
            raw.Guid,
            raw.IconId,
            raw.IsOffensive,
            raw.IsAssistance,
            raw.DurationAttributeId,
            raw.DischargeAttributeId,
            raw.RangeAttributeId,
            raw.DisallowAutoRepeat,
            raw.Published,
            raw.DisplayName?.En?.Trim(),
            raw.IsWarpSafe,
            raw.RangeChance,
            raw.ElectronicChance,
            raw.PropulsionChance,
            raw.Distribution,
            raw.ModifierInfoJson);
    }
}
