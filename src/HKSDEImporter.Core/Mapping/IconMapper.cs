using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class IconMapper : IMapper<RawIcon, Icon>
{
    public Icon Map(RawIcon raw)
    {
        return new Icon(
            raw.Key,
            raw.IconFile?.Trim(),
            raw.Description?.En?.Trim());
    }
}
