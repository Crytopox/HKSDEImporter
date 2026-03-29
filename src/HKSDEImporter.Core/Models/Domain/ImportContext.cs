using System.Collections.ObjectModel;
using HKSDEImporter.Core.Options;

namespace HKSDEImporter.Core.Models.Domain;

public sealed class ImportContext
{
    private readonly Dictionary<string, long> _rowCounts = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _warnings = [];
    private readonly List<string> _errors = [];

    public ImportContext(ImportOptions options)
    {
        Options = options;
        StartedAtUtc = DateTime.UtcNow;
    }

    public ImportOptions Options { get; }
    public DateTime StartedAtUtc { get; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? ResolvedInputDirectory { get; set; }

    public IReadOnlyDictionary<string, long> RowCounts => new ReadOnlyDictionary<string, long>(_rowCounts);
    public IReadOnlyList<string> Warnings => _warnings;
    public IReadOnlyList<string> Errors => _errors;

    public TimeSpan Duration => (CompletedAtUtc ?? DateTime.UtcNow) - StartedAtUtc;

    public void SetRowCount(string entityName, long count) => _rowCounts[entityName] = count;

    public void AddWarning(string warning) => _warnings.Add(warning);

    public void AddError(string error) => _errors.Add(error);

    public void Complete() => CompletedAtUtc = DateTime.UtcNow;
}
