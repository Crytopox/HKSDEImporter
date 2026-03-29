using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Core.ImportSteps;

public sealed class ImportIndustryBlueprintsStep : IImportStep
{
    private readonly IRawSdeReader _reader;
    private readonly IndustryMapper _mapper;
    private readonly IValidator<IndustryBlueprint> _validator;
    private readonly IIndustryWriter _writer;
    private readonly IImportObserver _observer;

    public ImportIndustryBlueprintsStep(IRawSdeReader reader, IndustryMapper mapper, IValidator<IndustryBlueprint> validator, IIndustryWriter writer, IImportObserver observer)
    {
        _reader = reader;
        _mapper = mapper;
        _validator = validator;
        _writer = writer;
        _observer = observer;
    }

    public string Name => "Import Industry Blueprints";

    public async Task ExecuteAsync(ImportContext context, CancellationToken cancellationToken)
    {
        var total = await _reader.CountBlueprintsAsync(context.ResolvedInputDirectory!, cancellationToken);
        _observer.OnStepStarted(Name, total);

        var blueprints = new List<IndustryBlueprint>();
        var activities = new List<IndustryActivity>();
        var materials = new List<IndustryActivityMaterial>();
        var products = new List<IndustryActivityProduct>();
        var probabilities = new List<IndustryActivityProbability>();
        var skills = new List<IndustryActivitySkill>();

        var seenBlueprints = new HashSet<int>();
        var seenActivities = new HashSet<string>(StringComparer.Ordinal);
        var seenMaterials = new HashSet<string>(StringComparer.Ordinal);
        var seenProducts = new HashSet<string>(StringComparer.Ordinal);
        var seenProbabilities = new HashSet<string>(StringComparer.Ordinal);
        var seenSkills = new HashSet<string>(StringComparer.Ordinal);

        var processed = 0L;
        await foreach (var raw in _reader.ReadBlueprintsAsync(context.ResolvedInputDirectory!, cancellationToken))
        {
            processed++;
            if (processed % 1000 == 0) _observer.OnStepProgress(Name, processed);

            var blueprint = _mapper.MapBlueprint(raw);
            if (!seenBlueprints.Add(blueprint.TypeId))
            {
                var warning = $"Duplicate blueprint type id {blueprint.TypeId}. Keeping first occurrence.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            var validation = _validator.Validate(blueprint);
            if (!validation.IsValid)
            {
                var warning = validation.Message ?? "Invalid blueprint row.";
                context.AddWarning(warning);
                _observer.OnWarning(warning);
                continue;
            }

            blueprints.Add(blueprint);

            foreach (var activity in _mapper.MapActivities(raw))
            {
                var key = $"{activity.TypeId}:{activity.ActivityId}";
                if (seenActivities.Add(key)) activities.Add(activity);
            }

            foreach (var material in _mapper.MapMaterials(raw))
            {
                var key = $"{material.TypeId}:{material.ActivityId}:{material.MaterialTypeId}";
                if (seenMaterials.Add(key)) materials.Add(material);
            }

            foreach (var product in _mapper.MapProducts(raw))
            {
                var key = $"{product.TypeId}:{product.ActivityId}:{product.ProductTypeId}";
                if (seenProducts.Add(key)) products.Add(product);
            }

            foreach (var probability in _mapper.MapProbabilities(raw))
            {
                var key = $"{probability.TypeId}:{probability.ActivityId}:{probability.ProductTypeId}";
                if (seenProbabilities.Add(key)) probabilities.Add(probability);
            }

            foreach (var skill in _mapper.MapSkills(raw))
            {
                var key = $"{skill.TypeId}:{skill.ActivityId}:{skill.SkillId}";
                if (seenSkills.Add(key)) skills.Add(skill);
            }
        }

        await _writer.WriteAsync(context.Options.OutputPath, blueprints, activities, materials, products, probabilities, skills, cancellationToken);

        context.SetRowCount("industryBlueprints", blueprints.Count);
        context.SetRowCount("industryActivity", activities.Count);
        context.SetRowCount("industryActivityMaterials", materials.Count);
        context.SetRowCount("industryActivityProducts", products.Count);
        context.SetRowCount("industryActivityProbabilities", probabilities.Count);
        context.SetRowCount("industryActivitySkills", skills.Count);
        _observer.OnStepProgress(Name, processed);
    }
}
