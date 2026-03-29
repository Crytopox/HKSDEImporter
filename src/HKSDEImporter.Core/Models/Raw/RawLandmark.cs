namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawLandmark
{
    public int Key { get; init; }
    public string? NameEn { get; init; }
    public string? DescriptionEn { get; init; }
    public int? IconId { get; init; }
    public RawPosition3D? Position { get; init; }
}
