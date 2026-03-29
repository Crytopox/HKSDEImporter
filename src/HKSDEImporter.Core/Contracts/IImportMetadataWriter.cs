namespace HKSDEImporter.Core.Contracts;

public interface IImportMetadataWriter
{
    Task WriteAsync(
        string outputPath,
        DateTime startedAtUtc,
        DateTime completedAtUtc,
        IReadOnlyDictionary<string, long> rowCounts,
        IReadOnlyList<string> warnings,
        IReadOnlyList<string> errors,
        CancellationToken cancellationToken);
}
