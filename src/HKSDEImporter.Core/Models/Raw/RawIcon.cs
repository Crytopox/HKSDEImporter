namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawIcon
{
    public int Key { get; init; }
    public string? IconFile { get; init; }
    public RawLocalizedText? Description { get; init; }
}
