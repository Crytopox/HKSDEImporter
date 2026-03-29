using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportCompatibilityTablesStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly ICompatibilityWriter _writer;
    private readonly IImportObserver _observer;

    public ImportCompatibilityTablesStep(IRawSdeReader reader, ICompatibilityWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Build Compatibility Tables";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total =
            await _reader.CountTypesAsync(context.ResolvedInputDirectory!, cancellationToken) +
            await _reader.CountNpcCorporationsAsync(context.ResolvedInputDirectory!, cancellationToken);

        _observer.OnStepStarted(Name, total);

        var metaTypes = new List<MetaType>();
        var metaSeen = new HashSet<int>();
        var corpDivisions = new List<NpcCorporationDivisionLink>();
        var divisionSeen = new HashSet<string>(StringComparer.Ordinal);
        var processed = 0L;

        await foreach (var type in _reader.ReadTypesAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (type.Key <= 0 || !metaSeen.Add(type.Key))
            {
                continue;
            }

            if (type.VariationParentTypeId.HasValue || type.MetaGroupId.HasValue)
            {
                metaTypes.Add(new MetaType(type.Key, type.VariationParentTypeId, type.MetaGroupId));
            }
        }

        await foreach (var corp in _reader.ReadNpcCorporationsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            foreach (var division in corp.Divisions ?? [])
            {
                if (corp.Key <= 0 || division.DivisionId <= 0)
                {
                    continue;
                }

                var key = $"{corp.Key}:{division.DivisionId}";
                if (!divisionSeen.Add(key))
                {
                    continue;
                }

                corpDivisions.Add(new NpcCorporationDivisionLink(corp.Key, division.DivisionId, division.Size));
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, metaTypes, corpDivisions, cancellationToken);

        context.SetRowCount("invMetaTypes", metaTypes.Count);
        context.SetRowCount("crpNPCCorporationDivisions", corpDivisions.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
