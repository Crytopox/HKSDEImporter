using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IStationWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<StationOperation> operations,
        IReadOnlyCollection<StationOperationService> operationServices,
        IReadOnlyCollection<StationService> services,
        IReadOnlyCollection<Station> stations,
        CancellationToken cancellationToken);
}
