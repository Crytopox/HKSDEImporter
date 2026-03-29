using System.IO.Compression;
using HKSDEImporter.Core.Contracts;

namespace HKSDEImporter.Infrastructure.Json.FileSystem;

public sealed class DirectDownloadSdeSourceProvider : ISdeSourceProvider
{
    private const string LatestJsonlZipUrl = "https://developers.eveonline.com/static-data/eve-online-static-data-latest-jsonl.zip";

    private readonly HttpClient _httpClient;

    public DirectDownloadSdeSourceProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SdeSourceHandle> PrepareAsync(CancellationToken cancellationToken)
    {
        var rootTempDirectory = Path.Combine(Path.GetTempPath(), "hksdeimporter", Guid.NewGuid().ToString("N"));
        var extractDirectory = Path.Combine(rootTempDirectory, "extracted");
        Directory.CreateDirectory(extractDirectory);

        var zipPath = Path.Combine(rootTempDirectory, "sde-latest-jsonl.zip");

        using var response = await _httpClient.GetAsync(LatestJsonlZipUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using (var zipStream = await response.Content.ReadAsStreamAsync(cancellationToken))
        await using (var output = File.Create(zipPath))
        {
            await zipStream.CopyToAsync(output, cancellationToken);
        }

        ZipFile.ExtractToDirectory(zipPath, extractDirectory);

        return new TemporaryDirectorySdeSourceHandle(extractDirectory, rootTempDirectory);
    }

    private sealed class TemporaryDirectorySdeSourceHandle : SdeSourceHandle
    {
        private readonly string _temporaryRoot;

        public TemporaryDirectorySdeSourceHandle(string rootDirectory, string temporaryRoot)
            : base(rootDirectory)
        {
            _temporaryRoot = temporaryRoot;
        }

        public override ValueTask DisposeAsync()
        {
            if (Directory.Exists(_temporaryRoot))
            {
                Directory.Delete(_temporaryRoot, recursive: true);
            }

            return ValueTask.CompletedTask;
        }
    }
}
