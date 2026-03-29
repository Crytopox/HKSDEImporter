using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportTypeDogmaStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly TypeDogmaMapper _mapper;
    private readonly ITypeDogmaWriter _writer;
    private readonly IImportObserver _observer;

    public ImportTypeDogmaStep(IRawSdeReader reader, TypeDogmaMapper mapper, ITypeDogmaWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Type Dogma";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountTypeDogmaAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var attributes = new List<TypeDogmaAttribute>();
        var effects = new List<TypeDogmaEffect>();
        var seenAttributes = new HashSet<string>(StringComparer.Ordinal);
        var seenEffects = new HashSet<string>(StringComparer.Ordinal);
        var processed = 0L;

        await foreach (var raw in _reader.ReadTypeDogmaAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            foreach (var attribute in _mapper.MapAttributes(raw))
            {
                var key = $"{attribute.TypeId}:{attribute.AttributeId}";
                if (seenAttributes.Add(key))
                {
                    attributes.Add(attribute);
                }
            }

            foreach (var effect in _mapper.MapEffects(raw))
            {
                var key = $"{effect.TypeId}:{effect.EffectId}";
                if (seenEffects.Add(key))
                {
                    effects.Add(effect);
                }
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, attributes, effects, cancellationToken);
        context.SetRowCount("dgmTypeAttributes", attributes.Count);
        context.SetRowCount("dgmTypeEffects", effects.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
