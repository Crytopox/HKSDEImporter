using HKSDEImporter.Core.Options;

namespace HKSDEImporter.Console;

public static class CliOptionsParser
{
    public static bool IsHelpRequested(string[] args)
    {
        return args.Any(arg => arg is "-h" or "--help");
    }

    public static ImportOptions ParseOrThrow(string[] args)
    {
        var inputMode = SdeInputMode.Direct;
        string? inputPath = null;
        string outputPath = "./eve.db";
        var overwrite = false;
        var verbose = false;

        for (var i = 0; i < args.Length; i++)
        {
            var token = args[i].Trim();
            switch (token)
            {
                case "--source":
                    EnsureNextValue(args, i, token);
                    i++;
                    inputMode = ParseInputMode(args[i]);
                    break;
                case "--input":
                    EnsureNextValue(args, i, token);
                    i++;
                    inputPath = args[i];
                    inputMode = SdeInputMode.Local;
                    break;
                case "--output":
                    EnsureNextValue(args, i, token);
                    i++;
                    outputPath = args[i];
                    break;
                case "--overwrite":
                    overwrite = true;
                    break;
                case "--verbose":
                    verbose = true;
                    break;
                default:
                    throw new ArgumentException($"Unknown argument '{token}'. Use --help for usage.");
            }
        }

        if (inputMode == SdeInputMode.Local && string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Local mode requires --input <path>.");
        }

        return new ImportOptions
        {
            InputMode = inputMode,
            InputPath = inputPath,
            OutputPath = outputPath,
            Overwrite = overwrite,
            Verbose = verbose
        };
    }

    public static string BuildHelpText()
    {
        return """
HKSDEImporter - EVE Online SDE JSONL -> SQLite importer

Usage:
  hksdeimporter --output ./eve.db [--overwrite] [--verbose]
  hksdeimporter --source direct --output ./eve.db [--overwrite] [--verbose]
  hksdeimporter --source local --input ./.local-sde-copy --output ./eve.db [--overwrite] [--verbose]

Options:
  --source <direct|local>  Input source mode (default: direct)
  --input <path>           Local SDE JSONL directory (implies --source local)
  --output <path>          Output SQLite file path (default: ./eve.db)
  --overwrite              Delete output DB if it already exists
  --verbose                Print extended warning details
  -h, --help               Show help
""";
    }

    private static void EnsureNextValue(string[] args, int index, string flag)
    {
        if (index + 1 >= args.Length || args[index + 1].StartsWith("--", StringComparison.Ordinal))
        {
            throw new ArgumentException($"Missing value for {flag}.");
        }
    }

    private static SdeInputMode ParseInputMode(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "direct" => SdeInputMode.Direct,
            "local" => SdeInputMode.Local,
            _ => throw new ArgumentException($"Invalid --source value '{value}'. Expected direct or local.")
        };
    }
}
