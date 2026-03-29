using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class ControlTowerResourceMapper
{
    public IEnumerable<ControlTowerResource> Map(RawControlTowerResourceSet raw)
    {
        foreach (var resource in raw.Resources ?? [])
        {
            yield return new ControlTowerResource(
                raw.ControlTowerTypeId,
                resource.ResourceTypeId,
                resource.Purpose,
                resource.Quantity,
                resource.MinSecurityLevel,
                resource.FactionId);
        }
    }
}
