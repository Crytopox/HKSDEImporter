using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ISkinWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<Skin> skins, IReadOnlyCollection<SkinMaterial> materials, IReadOnlyCollection<SkinShip> skinShips, IReadOnlyCollection<SkinLicense> licenses, CancellationToken cancellationToken);
}
