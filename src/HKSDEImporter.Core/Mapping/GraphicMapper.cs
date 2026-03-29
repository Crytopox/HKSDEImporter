using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class GraphicMapper : IMapper<RawGraphic, Graphic>
{
    public Graphic Map(RawGraphic raw)
    {
        return new Graphic(
            raw.Key,
            raw.SofFactionName?.Trim(),
            raw.GraphicFile?.Trim(),
            raw.SofHullName?.Trim(),
            raw.SofRaceName?.Trim(),
            raw.Description?.En?.Trim());
    }
}
