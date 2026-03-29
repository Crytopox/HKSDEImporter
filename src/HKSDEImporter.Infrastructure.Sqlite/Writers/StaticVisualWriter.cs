using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class StaticVisualWriter : IStaticVisualWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public StaticVisualWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<Graphic> graphics, IReadOnlyCollection<Icon> icons, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var graphicsCommand = connection.CreateCommand();
        graphicsCommand.Transaction = transaction;
        graphicsCommand.CommandText = "INSERT INTO eveGraphics (graphicID, sofFactionName, graphicFile, sofHullName, sofRaceName, description) VALUES ($graphicID, $sofFactionName, $graphicFile, $sofHullName, $sofRaceName, $description);";

        var graphicId = graphicsCommand.CreateParameter(); graphicId.ParameterName = "$graphicID"; graphicsCommand.Parameters.Add(graphicId);
        var sofFactionName = graphicsCommand.CreateParameter(); sofFactionName.ParameterName = "$sofFactionName"; graphicsCommand.Parameters.Add(sofFactionName);
        var graphicFile = graphicsCommand.CreateParameter(); graphicFile.ParameterName = "$graphicFile"; graphicsCommand.Parameters.Add(graphicFile);
        var sofHullName = graphicsCommand.CreateParameter(); sofHullName.ParameterName = "$sofHullName"; graphicsCommand.Parameters.Add(sofHullName);
        var sofRaceName = graphicsCommand.CreateParameter(); sofRaceName.ParameterName = "$sofRaceName"; graphicsCommand.Parameters.Add(sofRaceName);
        var graphicDescription = graphicsCommand.CreateParameter(); graphicDescription.ParameterName = "$description"; graphicsCommand.Parameters.Add(graphicDescription);

        foreach (var row in graphics)
        {
            graphicId.Value = row.GraphicId;
            sofFactionName.Value = string.IsNullOrWhiteSpace(row.SofFactionName) ? DBNull.Value : row.SofFactionName;
            graphicFile.Value = string.IsNullOrWhiteSpace(row.GraphicFile) ? DBNull.Value : row.GraphicFile;
            sofHullName.Value = string.IsNullOrWhiteSpace(row.SofHullName) ? DBNull.Value : row.SofHullName;
            sofRaceName.Value = string.IsNullOrWhiteSpace(row.SofRaceName) ? DBNull.Value : row.SofRaceName;
            graphicDescription.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            await graphicsCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        using var iconsCommand = connection.CreateCommand();
        iconsCommand.Transaction = transaction;
        iconsCommand.CommandText = "INSERT INTO eveIcons (iconID, iconFile, description) VALUES ($iconID, $iconFile, $description);";

        var iconId = iconsCommand.CreateParameter(); iconId.ParameterName = "$iconID"; iconsCommand.Parameters.Add(iconId);
        var iconFile = iconsCommand.CreateParameter(); iconFile.ParameterName = "$iconFile"; iconsCommand.Parameters.Add(iconFile);
        var iconDescription = iconsCommand.CreateParameter(); iconDescription.ParameterName = "$description"; iconsCommand.Parameters.Add(iconDescription);

        foreach (var row in icons)
        {
            iconId.Value = row.IconId;
            iconFile.Value = string.IsNullOrWhiteSpace(row.IconFile) ? DBNull.Value : row.IconFile;
            iconDescription.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
            await iconsCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
