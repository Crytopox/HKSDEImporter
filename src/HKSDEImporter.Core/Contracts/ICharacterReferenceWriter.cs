using HKSDEImporter.Core.Models.Domain;

namespace HKSDEImporter.Core.Contracts;

public interface ICharacterReferenceWriter
{
    Task WriteAsync(
        string outputPath,
        IReadOnlyCollection<Faction> factions,
        IReadOnlyCollection<Race> races,
        IReadOnlyCollection<Bloodline> bloodlines,
        IReadOnlyCollection<Ancestry> ancestries,
        IReadOnlyCollection<CharacterAttribute> attributes,
        CancellationToken cancellationToken);
}
