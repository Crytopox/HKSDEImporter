using HKSDEImporter.Console.UI;
using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.ImportSteps;
using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Models.Raw;
using HKSDEImporter.Core.Options;
using HKSDEImporter.Core.Services;
using HKSDEImporter.Core.Validation;
using HKSDEImporter.Infrastructure.Json.FileSystem;
using HKSDEImporter.Infrastructure.Json.Readers;
using HKSDEImporter.Infrastructure.Sqlite;
using HKSDEImporter.Infrastructure.Sqlite.Connections;
using HKSDEImporter.Infrastructure.Sqlite.Schema;
using HKSDEImporter.Infrastructure.Sqlite.Writers;
using Spectre.Console;

namespace HKSDEImporter.Console;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            if (CliOptionsParser.IsHelpRequested(args))
            {
                AnsiConsole.WriteLine(CliOptionsParser.BuildHelpText());
                return 0;
            }

            var options = args.Length == 0
                ? PromptForOptions()
                : CliOptionsParser.ParseOrThrow(args);

            ValidateOptions(options);

            var observer = new SpectreImportObserver(options.Verbose);
            var coordinator = BuildCoordinator(options, observer);

            await observer.RunWithProgressAsync(() => coordinator.RunAsync(options, CancellationToken.None));
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Import failed:[/] {Markup.Escape(ex.Message)}");
            return 1;
        }
    }

    private static ImportOptions PromptForOptions()
    {
        var modeSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select SDE source mode")
                .AddChoices(
                [
                    "Direct download from EVE (recommended)",
                    "Local directory"
                ]));

        var inputMode = modeSelection.StartsWith("Direct", StringComparison.Ordinal)
            ? SdeInputMode.Direct
            : SdeInputMode.Local;

        var inputPath = inputMode == SdeInputMode.Local
            ? AnsiConsole.Prompt(new TextPrompt<string>("Local SDE JSONL directory path:"))
            : null;

        var outputPath = AnsiConsole.Prompt(
            new TextPrompt<string>("Output SQLite path:")
                .DefaultValue("./eve.db"));

        var overwrite = AnsiConsole.Confirm("Overwrite output DB if it exists?", true);
        var verbose = AnsiConsole.Confirm("Verbose warning output?", false);

        return new ImportOptions
        {
            InputMode = inputMode,
            InputPath = inputPath,
            OutputPath = outputPath,
            Overwrite = overwrite,
            Verbose = verbose
        };
    }

    private static void ValidateOptions(ImportOptions options)
    {
        if (options.InputMode == SdeInputMode.Local)
        {
            if (string.IsNullOrWhiteSpace(options.InputPath))
            {
                throw new ArgumentException("Local mode requires an input path.");
            }

            if (!Directory.Exists(options.InputPath))
            {
                throw new DirectoryNotFoundException($"Input directory was not found: '{options.InputPath}'.");
            }
        }

        if (string.IsNullOrWhiteSpace(options.OutputPath))
        {
            throw new ArgumentException("Output path is required.");
        }
    }

    private static ImportCoordinator BuildCoordinator(ImportOptions options, IImportObserver observer)
    {
        var rawReader = new JsonLinesSdeReader();
        var connectionFactory = new SqliteConnectionFactory();

        ISdeSourceProvider sourceProvider = options.InputMode switch
        {
            SdeInputMode.Direct => new DirectDownloadSdeSourceProvider(new HttpClient()),
            SdeInputMode.Local => new LocalDirectorySdeSourceProvider(options.InputPath!),
            _ => throw new InvalidOperationException($"Unsupported input mode: {options.InputMode}")
        };

        var steps = new List<IImportStep>
        {
            new ImportCategoriesStep(rawReader, new CategoryMapper(), new CategoryValidator(), new CategoryWriter(connectionFactory), observer),
            new ImportGroupsStep(rawReader, new GroupMapper(), new GroupValidator(), new GroupWriter(connectionFactory), observer),
            new ImportTypesStep(rawReader, new TypeMapper(), new TypeValidator(), new TypeWriter(connectionFactory), observer)
        };

        return new ImportCoordinator(
            sourceProvider,
            new SqliteDatabaseInitializer(connectionFactory, new SqliteSchemaBuilder()),
            new ImportMetadataWriter(connectionFactory),
            steps,
            observer);
    }
}
