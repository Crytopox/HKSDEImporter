using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportSupplementalDataStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly ISupplementalDataWriter _writer;
    private readonly IImportObserver _observer;

    public ImportSupplementalDataStep(IRawSdeReader reader, ISupplementalDataWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Supplemental JSON Data";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var input = context.ResolvedInputDirectory!;
        var total =
            await _reader.CountCloneGradesAsync(input, cancellationToken) +
            await _reader.CountCompressibleTypesAsync(input, cancellationToken) +
            await _reader.CountDbuffCollectionsAsync(input, cancellationToken) +
            await _reader.CountDynamicItemAttributesAsync(input, cancellationToken) +
            await _reader.CountFreelanceJobSchemasAsync(input, cancellationToken) +
            await _reader.CountMercenaryTacticalOperationsAsync(input, cancellationToken) +
            await _reader.CountPlanetResourcesAsync(input, cancellationToken) +
            await _reader.CountSovereigntyUpgradesAsync(input, cancellationToken) +
            await _reader.CountTypeBonusesAsync(input, cancellationToken);

        _observer.OnStepStarted(Name, total);

        var cloneGrades = new List<CloneGrade>();
        var cloneGradeSkills = new List<CloneGradeSkill>();
        var compressibleTypes = new List<CompressibleType>();
        var dbuffCollections = new List<DbuffCollection>();
        var dynamicItemAttributes = new List<DynamicItemAttribute>();
        var freelanceJobSchemas = new List<FreelanceJobSchema>();
        var mercenaryTacticalOperations = new List<MercenaryTacticalOperation>();
        var planetResources = new List<PlanetResource>();
        var sovereigntyUpgrades = new List<SovereigntyUpgrade>();
        var typeBonuses = new List<TypeBonus>();
        var controlTowerResourcePurposes = new List<ControlTowerResourcePurpose>();

        var cloneGradeSeen = new HashSet<int>();
        var cloneSkillSeen = new HashSet<string>(StringComparer.Ordinal);
        var compressibleSeen = new HashSet<int>();
        var dbuffSeen = new HashSet<int>();
        var dynamicSeen = new HashSet<int>();
        var freelanceSeen = new HashSet<int>();
        var mercenarySeen = new HashSet<int>();
        var planetSeen = new HashSet<int>();
        var sovSeen = new HashSet<int>();
        var traitSeen = new HashSet<int>();
        var purposeSeen = new HashSet<int>();

        var processed = 0L;

        await foreach (var raw in _reader.ReadCloneGradesAsync(input, cancellationToken))
        {
            processed++;
            if (cloneGradeSeen.Add(raw.CloneGradeId) && raw.CloneGradeId > 0)
            {
                cloneGrades.Add(new CloneGrade(raw.CloneGradeId, raw.Name));
            }

            foreach (var skill in raw.Skills)
            {
                var key = $"{raw.CloneGradeId}:{skill.TypeId}";
                if (!cloneSkillSeen.Add(key) || raw.CloneGradeId <= 0 || skill.TypeId <= 0)
                {
                    continue;
                }

                cloneGradeSkills.Add(new CloneGradeSkill(raw.CloneGradeId, skill.TypeId, skill.Level));
            }
        }

        await foreach (var raw in _reader.ReadCompressibleTypesAsync(input, cancellationToken))
        {
            processed++;
            if (raw.TypeId > 0 && raw.CompressedTypeId > 0 && compressibleSeen.Add(raw.TypeId))
            {
                compressibleTypes.Add(new CompressibleType(raw.TypeId, raw.CompressedTypeId));
            }
        }

        await foreach (var raw in _reader.ReadDbuffCollectionsAsync(input, cancellationToken))
        {
            processed++;
            if (raw.CollectionId > 0 && dbuffSeen.Add(raw.CollectionId))
            {
                dbuffCollections.Add(new DbuffCollection(
                    raw.CollectionId,
                    raw.AggregateMode,
                    raw.OperationName,
                    raw.ShowOutputValueInUi,
                    raw.DeveloperDescription,
                    raw.DisplayNameEn,
                    raw.RawJson));
            }
        }

        await foreach (var raw in _reader.ReadDynamicItemAttributesAsync(input, cancellationToken))
        {
            processed++;
            if (raw.TypeId > 0 && dynamicSeen.Add(raw.TypeId))
            {
                dynamicItemAttributes.Add(new DynamicItemAttribute(raw.TypeId, raw.RawJson));
            }
        }

        await foreach (var raw in _reader.ReadFreelanceJobSchemasAsync(input, cancellationToken))
        {
            processed++;
            if (raw.SchemaId > 0 && freelanceSeen.Add(raw.SchemaId))
            {
                freelanceJobSchemas.Add(new FreelanceJobSchema(raw.SchemaId, raw.RawJson));
            }
        }

        await foreach (var raw in _reader.ReadMercenaryTacticalOperationsAsync(input, cancellationToken))
        {
            processed++;
            if (raw.OperationId > 0 && mercenarySeen.Add(raw.OperationId))
            {
                mercenaryTacticalOperations.Add(new MercenaryTacticalOperation(
                    raw.OperationId,
                    raw.NameEn,
                    raw.DescriptionEn,
                    raw.AnarchyImpact,
                    raw.DevelopmentImpact,
                    raw.InfomorphBonus,
                    raw.RawJson));
            }
        }

        await foreach (var raw in _reader.ReadPlanetResourcesAsync(input, cancellationToken))
        {
            processed++;
            if (raw.TypeId > 0 && planetSeen.Add(raw.TypeId))
            {
                planetResources.Add(new PlanetResource(raw.TypeId, raw.Power, raw.Workforce, raw.RawJson));
            }
        }

        await foreach (var raw in _reader.ReadSovereigntyUpgradesAsync(input, cancellationToken))
        {
            processed++;
            if (raw.TypeId > 0 && sovSeen.Add(raw.TypeId))
            {
                sovereigntyUpgrades.Add(new SovereigntyUpgrade(
                    raw.TypeId,
                    raw.MutuallyExclusiveGroup,
                    raw.PowerAllocation,
                    raw.WorkforceAllocation,
                    raw.FuelTypeId,
                    raw.FuelHourlyUpkeep,
                    raw.FuelStartupCost,
                    raw.RawJson));
            }
        }

        await foreach (var raw in _reader.ReadTypeBonusesAsync(input, cancellationToken))
        {
            processed++;
            if (raw.TypeId > 0 && traitSeen.Add(raw.TypeId))
            {
                typeBonuses.Add(new TypeBonus(raw.TypeId, raw.RawJson));
            }
        }

        await foreach (var raw in _reader.ReadControlTowerResourcesAsync(input, cancellationToken))
        {
            foreach (var resource in raw.Resources ?? [])
            {
                if (resource.Purpose.HasValue && purposeSeen.Add(resource.Purpose.Value))
                {
                    controlTowerResourcePurposes.Add(new ControlTowerResourcePurpose(resource.Purpose.Value, null));
                }
            }
        }

        await _writer.WriteAsync(
            context.Options.OutputPath,
            cloneGrades,
            cloneGradeSkills,
            compressibleTypes,
            dbuffCollections,
            dynamicItemAttributes,
            freelanceJobSchemas,
            mercenaryTacticalOperations,
            planetResources,
            sovereigntyUpgrades,
            typeBonuses,
            controlTowerResourcePurposes,
            cancellationToken);

        context.SetRowCount("chrCloneGrades", cloneGrades.Count);
        context.SetRowCount("chrCloneGradeSkills", cloneGradeSkills.Count);
        context.SetRowCount("invCompressibleTypes", compressibleTypes.Count);
        context.SetRowCount("dgmBuffCollections", dbuffCollections.Count);
        context.SetRowCount("dgmDynamicItemAttributes", dynamicItemAttributes.Count);
        context.SetRowCount("frtFreelanceJobSchemas", freelanceJobSchemas.Count);
        context.SetRowCount("mercenaryTacticalOperations", mercenaryTacticalOperations.Count);
        context.SetRowCount("planetResources", planetResources.Count);
        context.SetRowCount("sovSovereigntyUpgrades", sovereigntyUpgrades.Count);
        context.SetRowCount("invTraits", typeBonuses.Count);
        context.SetRowCount("invControlTowerResourcePurposes", controlTowerResourcePurposes.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
