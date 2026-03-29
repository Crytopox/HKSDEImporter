using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class RaceMapper : IMapper<RawRace, Race>
{
    public Race Map(RawRace raw)
    {
        return new Race(
            raw.Key,
            raw.Name?.En?.Trim() ?? string.Empty,
            raw.Description?.En?.Trim(),
            raw.IconId,
            raw.ShortDescription?.En?.Trim());
    }
}
