namespace HKSDEImporter.Core.Models.Raw;

public sealed class RawGraphic
{
    public int Key { get; init; }
    public string? GraphicFile { get; init; }
    public string? SofFactionName { get; init; }
    public string? SofHullName { get; init; }
    public string? SofRaceName { get; init; }
    public RawLocalizedText? Description { get; init; }
}
