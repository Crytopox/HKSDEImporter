using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class TypeMapper : IMapper<RawType, TypeItem>
{
    public TypeItem Map(RawType raw)
    {
        return new TypeItem(
            raw.Key,
            raw.GroupId,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim(),
            raw.Published,
            raw.PortionSize,
            raw.IconId,
            raw.SoundId,
            raw.GraphicId,
            raw.MarketGroupId,
            raw.RaceId,
            raw.Mass,
            raw.Volume,
            raw.Capacity,
            raw.Radius,
            raw.BasePrice);
    }
}
