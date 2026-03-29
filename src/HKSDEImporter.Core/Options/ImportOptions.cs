namespace HKSDEImporter.Core.Options;

public sealed class ImportOptions
{
    public SdeInputMode InputMode { get; init; } = SdeInputMode.Direct;
    public string? InputPath { get; init; }
    public string OutputPath { get; init; } = "eve-hk-sde.db";
    public bool Overwrite { get; init; }
    public bool Verbose { get; init; }
}
