using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface IIndustryWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<IndustryBlueprint> blueprints,
        IReadOnlyCollection<IndustryActivity> activities,
        IReadOnlyCollection<IndustryActivityMaterial> materials,
        IReadOnlyCollection<IndustryActivityProduct> products,
        IReadOnlyCollection<IndustryActivityProbability> probabilities,
        IReadOnlyCollection<IndustryActivitySkill> skills,
        CancellationToken cancellationToken);
}
