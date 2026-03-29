using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportSdeBuildInfoStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly ISdeBuildInfoWriter _writer;
    private readonly IImportObserver _observer;

    public ImportSdeBuildInfoStep(IRawSdeReader reader, ISdeBuildInfoWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import SDE Build Info";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountSdeBuildInfoAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var rows = new List<SdeBuildInfo>();
        await foreach (var raw in _reader.ReadSdeBuildInfoAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            if (string.IsNullOrWhiteSpace(raw.SourceKey) || !raw.BuildNumber.HasValue || !raw.ReleaseDateUtc.HasValue)
            {
                continue;
            }

            rows.Add(new SdeBuildInfo(raw.SourceKey!, raw.BuildNumber.Value, raw.ReleaseDateUtc.Value));
        }

        await _writer.WriteAsync(context.Options.OutputPath, rows, cancellationToken);
        context.SetRowCount("_hki_sde_build", rows.Count);
        _observer.OnStepProgress(Name, rows.Count);
    }
}
