using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IDogmaUnitWriter
{
    Task WriteAsync(string outputPath, IReadOnlyCollection<DogmaUnit> units, CancellationToken cancellationToken);
}
