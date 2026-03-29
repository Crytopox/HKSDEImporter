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
                .DefaultValue("./eve-hk-sde.db"));

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
            new ImportTypesStep(rawReader, new TypeMapper(), new TypeValidator(), new TypeWriter(connectionFactory), observer),
            new ImportMarketGroupsStep(rawReader, new MarketGroupMapper(), new MarketGroupValidator(), new MarketGroupWriter(connectionFactory), observer),
            new ImportMetaGroupsStep(rawReader, new MetaGroupMapper(), new MetaGroupValidator(), new MetaGroupWriter(connectionFactory), observer),
            new ImportDogmaUnitsStep(rawReader, new DogmaUnitMapper(), new DogmaUnitValidator(), new DogmaUnitWriter(connectionFactory), observer),
            new ImportDogmaAttributeCategoriesStep(rawReader, new DogmaAttributeCategoryMapper(), new DogmaAttributeCategoryValidator(), new DogmaAttributeCategoryWriter(connectionFactory), observer),
            new ImportDogmaAttributesStep(rawReader, new DogmaAttributeMapper(), new DogmaAttributeValidator(), new DogmaAttributeWriter(connectionFactory), observer),
            new ImportDogmaEffectsStep(rawReader, new DogmaEffectMapper(), new DogmaEffectValidator(), new DogmaEffectWriter(connectionFactory), observer),
            new ImportTypeDogmaStep(rawReader, new TypeDogmaMapper(), new TypeDogmaWriter(connectionFactory), observer),
            new ImportTypeMaterialsStep(rawReader, new TypeMaterialMapper(), new TypeMaterialWriter(connectionFactory), observer),
            new ImportIndustryBlueprintsStep(rawReader, new IndustryMapper(), new IndustryBlueprintValidator(), new IndustryWriter(connectionFactory), observer),
            new ImportPlanetSchematicsStep(rawReader, new PlanetSchematicsMapper(), new PlanetSchematicValidator(), new PlanetSchematicsWriter(connectionFactory), observer),
            new ImportStaticVisualsStep(rawReader, new GraphicMapper(), new IconMapper(), new GraphicValidator(), new IconValidator(), new StaticVisualWriter(connectionFactory), observer),
            new ImportCharacterReferenceStep(rawReader, new FactionMapper(), new RaceMapper(), new BloodlineMapper(), new AncestryMapper(), new CharacterAttributeMapper(), new FactionValidator(), new RaceValidator(), new BloodlineValidator(), new AncestryValidator(), new CharacterAttributeValidator(), new CharacterReferenceWriter(connectionFactory), observer),
            new ImportCorporationReferenceStep(rawReader, new CorporationActivityMapper(), new NpcCorporationDivisionMapper(), new NpcCorporationMapper(), new CorporationActivityValidator(), new NpcCorporationDivisionValidator(), new NpcCorporationValidator(), new CorporationReferenceWriter(connectionFactory), observer),
            new ImportStationReferencesStep(rawReader, new StationMapper(), new StationOperationValidator(), new StationServiceValidator(), new StationValidator(), new StationWriter(connectionFactory), observer),
            new ImportMapDataStep(rawReader, new MapDataMapper(), new MapRegionValidator(), new MapConstellationValidator(), new MapSolarSystemValidator(), new MapLandmarkValidator(), new MapWriter(connectionFactory), observer),
            new ImportContrabandStep(rawReader, new ContrabandMapper(), new ContrabandTypeRuleValidator(), new ContrabandWriter(connectionFactory), observer),
            new ImportControlTowerResourcesStep(rawReader, new ControlTowerResourceMapper(), new ControlTowerResourceValidator(), new ControlTowerResourceWriter(connectionFactory), observer),
            new ImportSupplementalDataStep(rawReader, new SupplementalDataWriter(connectionFactory), observer),
            new ImportTranslationLanguagesStep(rawReader, new TranslationLanguageMapper(), new TranslationLanguageValidator(), new TranslationLanguageWriter(connectionFactory), observer),
            new ImportAgentReferencesStep(rawReader, new AgentTypeMapper(), new AgentInSpaceMapper(), new AgentTypeValidator(), new AgentInSpaceValidator(), new AgentReferenceWriter(connectionFactory), observer),
            new ImportCertificatesStep(rawReader, new CertificateMapper(), new CertValidator(), new CertMasteryValidator(), new CertSkillValidator(), new CertificateWriter(connectionFactory), observer),
            new ImportSkinsStep(rawReader, new SkinMapper(), new SkinValidator(), new SkinMaterialValidator(), new SkinShipValidator(), new SkinLicenseValidator(), new SkinWriter(connectionFactory), observer),
            new ImportNpcAgentDataStep(rawReader, new NpcAgentMapper(), new AgentValidator(), new ResearchAgentValidator(), new NpcCorporationResearchFieldValidator(), new NpcCorporationTradeValidator(), new NpcAgentWriter(connectionFactory), observer),
            new ImportSdeBuildInfoStep(rawReader, new SdeBuildInfoWriter(connectionFactory), observer),
            new ImportCompatibilityTablesStep(rawReader, new CompatibilityWriter(connectionFactory), observer)
        };

        return new ImportCoordinator(
            sourceProvider,
            new SqliteDatabaseInitializer(connectionFactory, new SqliteSchemaBuilder()),
            new ImportMetadataWriter(connectionFactory),
            steps,
            observer);
    }
}
