using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportPlanetSchematicsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly PlanetSchematicsMapper _mapper;
    private readonly IValidator<PlanetSchematic> _validator;
    private readonly IPlanetSchematicsWriter _writer;
    private readonly IImportObserver _observer;

    public ImportPlanetSchematicsStep(IRawSdeReader reader, PlanetSchematicsMapper mapper, IValidator<PlanetSchematic> validator, IPlanetSchematicsWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Planet Schematics";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountPlanetSchematicsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var schematics = new List<PlanetSchematic>();
        var pinMaps = new List<PlanetSchematicPinMap>();
        var typeMaps = new List<PlanetSchematicTypeMap>();

        var seenSchematic = new HashSet<int>();
        var seenPin = new HashSet<string>(StringComparer.Ordinal);
        var seenType = new HashSet<string>(StringComparer.Ordinal);

        var processed = 0L;
        await foreach (var raw in _reader.ReadPlanetSchematicsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 100 == 0) _observer.OnStepProgress(Name, processed);

            var mapped = _mapper.MapSchematic(raw);
            if (!seenSchematic.Add(mapped.SchematicId))
            {
                var warning = $"Duplicate planet schematic id {mapped.SchematicId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var validation = _validator.Validate(mapped);
            if (!validation.IsValid)
            {
                var warning = validation.Message ?? "Invalid planet schematic row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            schematics.Add(mapped);

            foreach (var pin in _mapper.MapPinMaps(raw))
            {
                var key = $"{pin.SchematicId}:{pin.PinTypeId}";
                if (seenPin.Add(key)) pinMaps.Add(pin);
            }

            foreach (var type in _mapper.MapTypeMaps(raw))
            {
                var key = $"{type.SchematicId}:{type.TypeId}:{type.IsInput}";
                if (seenType.Add(key)) typeMaps.Add(type);
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, schematics, pinMaps, typeMaps, cancellationToken);

        context.SetRowCount("planetSchematics", schematics.Count);
        context.SetRowCount("planetSchematicsPinMap", pinMaps.Count);
        context.SetRowCount("planetSchematicsTypeMap", typeMaps.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
