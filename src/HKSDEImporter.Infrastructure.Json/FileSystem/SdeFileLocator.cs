namespace HKSDEImporter.Infrastructure.Json.FileSystem;

internal static class SdeFileLocator
{
    public static string LocateRequiredFile(string rootDirectory, string fileName)
    {
        var directPath = Path.Combine(rootDirectory, fileName);
        if (File.Exists(directPath))
        {
            return directPath;
        }

        var recursive = Directory
            .EnumerateFiles(rootDirectory, fileName, SearchOption.AllDirectories)
            .FirstOrDefault();

        if (recursive is null)
        {
            throw new FileNotFoundException($"Required SDE file '{fileName}' was not found under '{rootDirectory}'.");
        }

        return recursive;
    }
}
