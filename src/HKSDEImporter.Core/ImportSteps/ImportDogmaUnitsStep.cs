using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportDogmaUnitsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawDogmaUnit, DogmaUnit> _mapper;
    private readonly IValidator<DogmaUnit> _validator;
    private readonly IDogmaUnitWriter _writer;
    private readonly IImportObserver _observer;

    public ImportDogmaUnitsStep(IRawSdeReader reader, IMapper<RawDogmaUnit, DogmaUnit> mapper, IValidator<DogmaUnit> validator, IDogmaUnitWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Dogma Units";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountDogmaUnitsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var items = new List<DogmaUnit>();
        var seen = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadDogmaUnitsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 200 == 0) _observer.OnStepProgress(Name, processed);
            var mapped = _mapper.Map(raw);
            if (!seen.Add(mapped.UnitId))
            {
                var warning = $"Duplicate dogma unit id {mapped.UnitId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid dogma unit row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            items.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, items, cancellationToken);
        context.SetRowCount("eveUnits", items.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
