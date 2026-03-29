using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportControlTowerResourcesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly ControlTowerResourceMapper _mapper;
    private readonly IValidator<ControlTowerResource> _validator;
    private readonly IControlTowerResourceWriter _writer;
    private readonly IImportObserver _observer;

    public ImportControlTowerResourcesStep(IRawSdeReader reader, ControlTowerResourceMapper mapper, IValidator<ControlTowerResource> validator, IControlTowerResourceWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Control Tower Resources";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountControlTowerResourcesAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var rows = new List<ControlTowerResource>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var processed = 0L;

        await foreach (var raw in _reader.ReadControlTowerResourcesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            foreach (var row in _mapper.Map(raw))
            {
                var key = $"{row.ControlTowerTypeId}:{row.ResourceTypeId}";
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
                    var warning = result.Message ?? "Invalid control tower resource row.";
                    context.AddWarning(warning);
                    _observer.OnWarning(warning);
                }
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, rows, cancellationToken);
        context.SetRowCount("invControlTowerResources", rows.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
