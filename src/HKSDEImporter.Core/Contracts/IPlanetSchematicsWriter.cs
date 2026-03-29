using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IPlanetSchematicsWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<PlanetSchematic> schematics,
        IReadOnlyCollection<PlanetSchematicPinMap> pinMaps,
        IReadOnlyCollection<PlanetSchematicTypeMap> typeMaps,
        CancellationToken cancellationToken);
}
