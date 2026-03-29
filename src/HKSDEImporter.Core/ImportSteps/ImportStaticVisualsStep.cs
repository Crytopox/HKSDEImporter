using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportStaticVisualsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IMapper<RawGraphic, Graphic> _graphicMapper;
    private readonly IMapper<RawIcon, Icon> _iconMapper;
    private readonly IValidator<Graphic> _graphicValidator;
    private readonly IValidator<Icon> _iconValidator;
    private readonly IStaticVisualWriter _writer;
    private readonly IImportObserver _observer;

    public ImportStaticVisualsStep(
        IRawSdeReader reader,
        IMapper<RawGraphic, Graphic> graphicMapper,
        IMapper<RawIcon, Icon> iconMapper,
        IValidator<Graphic> graphicValidator,
        IValidator<Icon> iconValidator,
        IStaticVisualWriter writer,
        IImportObserver observer)
    {
        _reader = reader;
        _graphicMapper = graphicMapper;
        _iconMapper = iconMapper;
        _graphicValidator = graphicValidator;
        _iconValidator = iconValidator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Static Visuals";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var totalGraphics = await _reader.CountGraphicsAsync(context.ResolvedInputDirectory!, cancellationToken);
        var totalIcons = await _reader.CountIconsAsync(context.ResolvedInputDirectory!, cancellationToken);
        var total = totalGraphics + totalIcons;

        _observer.OnStepStarted(Name, total);

        var graphics = new List<Graphic>();
        var icons = new List<Icon>();

        var seenGraphics = new HashSet<int>();
        var seenIcons = new HashSet<int>();
        var processed = 0L;

        await foreach (var raw in _reader.ReadGraphicsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _graphicMapper.Map(raw);
            if (!seenGraphics.Add(mapped.GraphicId))
            {
                var warning = $"Duplicate graphic id {mapped.GraphicId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _graphicValidator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid graphic row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            graphics.Add(mapped);
        }

        await foreach (var raw in _reader.ReadIconsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _iconMapper.Map(raw);
            if (!seenIcons.Add(mapped.IconId))
            {
                var warning = $"Duplicate icon id {mapped.IconId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var result = _iconValidator.Validate(mapped);
            if (!result.IsValid)
            {
                var warning = result.Message ?? "Invalid icon row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            icons.Add(mapped);
        }

        await _writer.WriteAsync(context.Options.OutputPath, graphics, icons, cancellationToken);

        context.SetRowCount("eveGraphics", graphics.Count);
        context.SetRowCount("eveIcons", icons.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
