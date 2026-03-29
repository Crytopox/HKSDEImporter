using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportContrabandStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly ContrabandMapper _mapper;
    private readonly IValidator<ContrabandTypeRule> _validator;
    private readonly IContrabandWriter _writer;
    private readonly IImportObserver _observer;

    public ImportContrabandStep(IRawSdeReader reader, ContrabandMapper mapper, IValidator<ContrabandTypeRule> validator, IContrabandWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Contraband Types";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountContrabandTypesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var rows = new List<ContrabandTypeRule>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var processed = 0L;

        await foreach (var raw in _reader.ReadContrabandTypesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            foreach (var row in _mapper.Map(raw))
            {
                var key = $"{row.FactionId}:{row.TypeId}";
                if (!seen.Add(key))
                {
                    continue;
                }

                var result = _validator.Validate(row);
                if (result.IsValid)
                {
                    rows.Add(row);
                }
                else
                {
                    var warning = result.Message ?? "Invalid contraband row.";
                    context.AddWarning(warning);
                    _observer.OnWarning(warning);
                }
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, rows, cancellationToken);
        context.SetRowCount("invContrabandTypes", rows.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
