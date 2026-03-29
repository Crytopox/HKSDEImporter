using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportMetaGroupsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawMetaGroup, MetaGroup> _mapper;
    private readonly IValidator<MetaGroup> _validator;
    private readonly IMetaGroupWriter _writer;
    private readonly IImportObserver _observer;

    public ImportMetaGroupsStep(IRawSdeReader reader, IMapper<RawMetaGroup, MetaGroup> mapper, IValidator<MetaGroup> validator, IMetaGroupWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Meta Groups";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountMetaGroupsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var items = new List<MetaGroup>();
        var seen = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadMetaGroupsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 200 == 0) _observer.OnStepProgress(Name, processed);
            var mapped = _mapper.Map(raw);
            if (!seen.Add(mapped.MetaGroupId))
            {
                var warning = $"Duplicate meta group id {mapped.MetaGroupId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid meta group row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            items.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, items, cancellationToken);
        context.SetRowCount("invMetaGroups", items.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
