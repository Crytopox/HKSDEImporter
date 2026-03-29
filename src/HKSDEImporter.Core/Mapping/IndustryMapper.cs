using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Core.Mapping;

public sealed class IndustryMapper
{
    public IndustryBlueprint MapBlueprint(RawBlueprint raw)
    {
        return new IndustryBlueprint(raw.TypeId, raw.MaxProductionLimit);
    }

    public IReadOnlyList<IndustryActivity> MapActivities(RawBlueprint raw)
    {
        var output = new List<IndustryActivity>(raw.Activities.Count);
        foreach (var activity in raw.Activities)
        {
            if (activity.Time.HasValue)
            {
                output.Add(new IndustryActivity(raw.TypeId, activity.ActivityId, activity.Time.Value));
            }
        }

        return output;
    }

    public IReadOnlyList<IndustryActivityMaterial> MapMaterials(RawBlueprint raw)
    {
        var output = new List<IndustryActivityMaterial>();
        foreach (var activity in raw.Activities)
        {
            foreach (var material in activity.Materials)
            {
                output.Add(new IndustryActivityMaterial(raw.TypeId, activity.ActivityId, material.TypeId, material.Quantity));
            }
        }

        return output;
    }

    public IReadOnlyList<IndustryActivityProduct> MapProducts(RawBlueprint raw)
    {
        var output = new List<IndustryActivityProduct>();
        foreach (var activity in raw.Activities)
        {
            foreach (var product in activity.Products)
            {
                output.Add(new IndustryActivityProduct(raw.TypeId, activity.ActivityId, product.TypeId, product.Quantity));
            }
        }

        return output;
    }

    public IReadOnlyList<IndustryActivityProbability> MapProbabilities(RawBlueprint raw)
    {
        var output = new List<IndustryActivityProbability>();
        foreach (var activity in raw.Activities)
        {
            foreach (var product in activity.Products)
            {
                if (product.Probability.HasValue)
                {
                    output.Add(new IndustryActivityProbability(raw.TypeId, activity.ActivityId, product.TypeId, product.Probability.Value));
                }
            }
        }

        return output;
    }

    public IReadOnlyList<IndustryActivitySkill> MapSkills(RawBlueprint raw)
    {
        var output = new List<IndustryActivitySkill>();
        foreach (var activity in raw.Activities)
        {
            foreach (var skill in activity.Skills)
            {
                output.Add(new IndustryActivitySkill(raw.TypeId, activity.ActivityId, skill.TypeId, skill.Level));
            }
        }

        return output;
    }
}
