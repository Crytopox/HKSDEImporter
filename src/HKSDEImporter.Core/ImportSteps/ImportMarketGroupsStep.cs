using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportMarketGroupsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawMarketGroup, MarketGroup> _mapper;
    private readonly IValidator<MarketGroup> _validator;
    private readonly IMarketGroupWriter _writer;
    private readonly IImportObserver _observer;

    public ImportMarketGroupsStep(IRawSdeReader reader, IMapper<RawMarketGroup, MarketGroup> mapper, IValidator<MarketGroup> validator, IMarketGroupWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Market Groups";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountMarketGroupsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var items = new List<MarketGroup>();
        var seen = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadMarketGroupsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);
            var mapped = _mapper.Map(raw);
            if (!seen.Add(mapped.MarketGroupId))
            {
                var warning = $"Duplicate market group id {mapped.MarketGroupId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid market group row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            items.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, items, cancellationToken);
        context.SetRowCount("invMarketGroups", items.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
