namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawSkin
{
    public int SkinId { get; init; }
    public string? InternalName { get; init; }
    public int? SkinMaterialId { get; init; }
    public List<int>? TypeIds { get; init; }
}
