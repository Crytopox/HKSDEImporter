using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportTypeMaterialsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly TypeMaterialMapper _mapper;
    private readonly ITypeMaterialWriter _writer;
    private readonly IImportObserver _observer;

    public ImportTypeMaterialsStep(IRawSdeReader reader, TypeMaterialMapper mapper, ITypeMaterialWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Type Materials";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountTypeMaterialsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var materials = new List<TypeMaterial>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var processed = 0L;

        await foreach (var raw in _reader.ReadTypeMaterialsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 2000 == 0) _observer.OnStepProgress(Name, processed);

            foreach (var material in _mapper.Map(raw))
            {
                var key = $"{material.TypeId}:{material.MaterialTypeId}";
                if (seen.Add(key))
                {
                    materials.Add(material);
                }
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, materials, cancellationToken);
        context.SetRowCount("invTypeMaterials", materials.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
