using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IStaticVisualWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<Graphic> graphics, IReadOnlyCollection<Icon> icons, CancellationToken cancellationToken);
}
