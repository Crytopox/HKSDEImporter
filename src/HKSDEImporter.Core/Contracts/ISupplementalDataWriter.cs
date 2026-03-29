using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ISupplementalDataWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<CloneGrade> cloneGrades,
        IReadOnlyCollection<CloneGradeSkill> cloneGradeSkills,
        IReadOnlyCollection<CompressibleType> compressibleTypes,
        IReadOnlyCollection<DbuffCollection> dbuffCollections,
        IReadOnlyCollection<DynamicItemAttribute> dynamicItemAttributes,
        IReadOnlyCollection<FreelanceJobSchema> freelanceJobSchemas,
        IReadOnlyCollection<MercenaryTacticalOperation> mercenaryTacticalOperations,
        IReadOnlyCollection<PlanetResource> planetResources,
        IReadOnlyCollection<SovereigntyUpgrade> sovereigntyUpgrades,
        IReadOnlyCollection<TypeBonus> typeBonuses,
        IReadOnlyCollection<ControlTowerResourcePurpose> controlTowerResourcePurposes,
        CancellationToken cancellationToken);
}
