using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Options;

namespace HKSDEImporter.Core.Services;

public sealed class ImportCoordinator
{
    private readonly ISdeSourceProvider _sourceProvider;
    private readonly IDatabaseInitializer _databaseInitializer;
    private readonly IImportMetadataWriter _metadataWriter;
    private readonly IReadOnlyCollection<IImportStep> _steps;
    private readonly IImportObserver _observer;

    public ImportCoordinator(
        ISdeSourceProvider sourceProvider,
        IDatabaseInitializer databaseInitializer,
        IImportMetadataWriter metadataWriter,
        IReadOnlyCollection<IImportStep> steps,
        IImportObserver observer)
    {
        _sourceProvider = sourceProvider;
        _databaseInitializer = databaseInitializer;
        _metadataWriter = metadataWriter;
        _steps = steps;
        _observer = observer;
    }

    public async Task<ImportContext> RunAsync(ImportOptions options, CancellationToken cancellationToken)
    {
        var context = new ImportContext(options);
        _observer.OnStarted(context);

        await using var source = await _sourceProvider.PrepareAsync(cancellationToken);
        context.ResolvedInputDirectory = source.RootDirectory;

        await _databaseInitializer.InitializeAsync(options.OutputPath, options.Overwrite, cancellationToken);

        foreach (var step in _steps)
        {
            await step.ExecuteAsync(context, cancellationToken);
        }

        context.Complete();

        await _metadataWriter.WriteAsync(
            options.OutputPath,
            context.StartedAtUtc,
            context.CompletedAtUtc ?? DateTime.UtcNow,
            context.RowCounts,
            context.Warnings,
            context.Errors,
            cancellationToken);

        _observer.OnCompleted(context);
        return context;
    }
}
