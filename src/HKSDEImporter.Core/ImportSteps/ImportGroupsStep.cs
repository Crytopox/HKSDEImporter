using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportGroupsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawGroup, Group> _mapper;
    private readonly IValidator<Group> _validator;
    private readonly IGroupWriter _writer;
    private readonly IImportObserver _observer;

    public ImportGroupsStep(
        IRawSdeReader reader,
        IMapper<RawGroup, Group> mapper,
        IValidator<Group> validator,
        IGroupWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Groups";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var totalCount = await _reader.CountGroupsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, totalCount);

        var groups = new List<Group>();
        var seenIds = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadGroupsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0)
            {
                _observer.OnStepProgress(Name, processed);
            }

            var mapped = _mapper.Map(raw);
            if (!seenIds.Add(mapped.GroupId))
            {
                var warning = $"Duplicate group id {mapped.GroupId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _validator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid group row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            groups.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, groups, cancellationToken);
        context.SetRowCount("groups", groups.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
