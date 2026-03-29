using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class SkinMapper
{
    public Skin MapSkin(RawSkin raw) => new(raw.SkinId, raw.InternalName?.Trim(), raw.SkinMaterialId);

    public SkinMaterial MapMaterial(RawSkinMaterial raw) => new(raw.SkinMaterialId, null, raw.MaterialSetId);

    public IEnumerable<SkinShip> MapSkinShips(RawSkin raw)
    {
        foreach (var typeId in raw.TypeIds ?? [])
        {
            yield return new SkinShip(raw.SkinId, typeId);
        }
    }

    public SkinLicense MapLicense(RawSkinLicense raw) => new(raw.LicenseTypeId, raw.Duration, raw.SkinId);
}
