using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IDogmaEffectWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaEffect> effects, CancellationToken cancellationToken);
}
