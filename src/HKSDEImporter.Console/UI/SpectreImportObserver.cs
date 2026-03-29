using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using Spectre.Console;

namespace HKSDEImporter.Console.UI;

public sealed class SpectreImportObserver : IImportObserver
{
    private readonly bool _verbose;
    private readonly object _sync = new();
    private readonly Dictionary<string, ProgressTask> _tasks = new(StringComparer.Ordinal);
    private readonly List<string> _warnings = [];
    private ProgressContext? _progressContext;

    public SpectreImportObserver(bool verbose)
    {
        _verbose = verbose;
    }

    public async Task<ImportContext> RunWithProgressAsync(Func<Task<ImportContext>> run)
    {
        ImportContext? result = null;

        await AnsiConsole
            .Progress()
            .AutoClear(true)
            .HideCompleted(true)
            .Columns(
            [
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new SpinnerColumn(),
                new RemainingTimeColumn()
            ])
            .StartAsync(async context =>
            {
                _progressContext = context;
                result = await run();
            });

        return result ?? throw new InvalidOperationException("Import did not produce a result.");
    }

    public void OnStarted(ImportContext context)
    {
        AnsiConsole.Write(
            new FigletText("HKSDE Importer")
                .LeftJustified()
                .Color(Color.CadetBlue));

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("Setting");
        table.AddColumn("Value");
        table.AddRow("Input mode", context.Options.InputMode.ToString());
        table.AddRow("Input path", context.Options.InputPath ?? "(download latest JSONL from EVE)");
        table.AddRow("Output DB", Path.GetFullPath(context.Options.OutputPath));
        table.AddRow("Overwrite", context.Options.Overwrite ? "Yes" : "No");
        table.AddRow("Verbose", context.Options.Verbose ? "Yes" : "No");
        AnsiConsole.Write(table);
        AnsiConsole.Write(new Rule("Import Progress").RuleStyle("grey"));
    }

    public void OnStepStarted(string stepName, long totalCount)
    {
        lock (_sync)
        {
            if (_progressContext is null)
            {
                return;
            }

            var safeTotal = Math.Max(totalCount, 1);
            _tasks[stepName] = _progressContext.AddTask($"[aqua]{Markup.Escape(stepName)}[/]", true, safeTotal);
        }
    }

    public void OnStepProgress(string stepName, long processedCount)
    {
        lock (_sync)
        {
            if (!_tasks.TryGetValue(stepName, out var task))
            {
                return;
            }

            task.Value = Math.Min(processedCount, task.MaxValue);
            if (processedCount >= task.MaxValue)
            {
                task.StopTask();
            }
        }
    }

    public void OnWarning(string warning)
    {
        lock (_sync)
        {
            _warnings.Add(warning);
        }
    }

    public void OnError(string error)
    {
        AnsiConsole.MarkupLine($"[red]Error:[/] {Markup.Escape(error)}");
    }

    public void OnCompleted(ImportContext context)
    {
        var summary = new Table().Border(TableBorder.HeavyHead);
        summary.AddColumn("Metric");
        summary.AddColumn("Value");
        summary.AddRow("Status", context.Errors.Count == 0 ? "Success" : "Completed with errors");
        summary.AddRow("Duration", $"{context.Duration.TotalSeconds:N1}s");
        summary.AddRow("Warnings", context.Warnings.Count.ToString());
        summary.AddRow("Errors", context.Errors.Count.ToString());
        summary.AddRow("Resolved input", context.ResolvedInputDirectory ?? "(n/a)");
        AnsiConsole.Write(new Rule("Summary").RuleStyle("green"));
        AnsiConsole.Write(summary);

        var rows = new BarChart().Width(80).Label("Rows Imported").CenterLabel();
        foreach (var item in context.RowCounts)
        {
            rows.AddItem(item.Key, item.Value, Color.CornflowerBlue);
        }

        AnsiConsole.Write(rows);

        if (_verbose && _warnings.Count > 0)
        {
            AnsiConsole.Write(new Rule("Warnings").RuleStyle("yellow"));
            foreach (var warning in _warnings.Take(25))
            {
                AnsiConsole.MarkupLine($"[yellow]-[/] {Markup.Escape(warning)}");
            }

            if (_warnings.Count > 25)
            {
                AnsiConsole.MarkupLine($"[yellow]... {_warnings.Count - 25} more warnings omitted.[/]");
            }
        }
    }
}
