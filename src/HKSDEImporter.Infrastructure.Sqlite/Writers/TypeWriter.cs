using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class TypeWriter : ITypeWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public TypeWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<TypeItem> types, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.CommandText =
            "INSERT INTO types (type_id, group_id, name, description, published, portion_size, icon_id, mass, radius, volume) " +
            "VALUES ($typeId, $groupId, $name, $description, $published, $portionSize, $iconId, $mass, $radius, $volume);";
        command.Transaction = transaction;

        var typeId = command.CreateParameter();
        typeId.ParameterName = "$typeId";
        command.Parameters.Add(typeId);

        var groupId = command.CreateParameter();
        groupId.ParameterName = "$groupId";
        command.Parameters.Add(groupId);

        var name = command.CreateParameter();
        name.ParameterName = "$name";
        command.Parameters.Add(name);

        var description = command.CreateParameter();
        description.ParameterName = "$description";
        command.Parameters.Add(description);

        var published = command.CreateParameter();
        published.ParameterName = "$published";
        command.Parameters.Add(published);

        var portionSize = command.CreateParameter();
        portionSize.ParameterName = "$portionSize";
        command.Parameters.Add(portionSize);

        var iconId = command.CreateParameter();
        iconId.ParameterName = "$iconId";
        command.Parameters.Add(iconId);

        var mass = command.CreateParameter();
        mass.ParameterName = "$mass";
        command.Parameters.Add(mass);

        var radius = command.CreateParameter();
        radius.ParameterName = "$radius";
        command.Parameters.Add(radius);

        var volume = command.CreateParameter();
        volume.ParameterName = "$volume";
        command.Parameters.Add(volume);

        foreach (var type in types)
        {
            cancellationToken.ThrowIfCancellationRequested();

            typeId.Value = type.TypeId;
            groupId.Value = type.GroupId;
            name.Value = type.Name;
            description.Value = string.IsNullOrWhiteSpace(type.Description) ? DBNull.Value : type.Description;
            published.Value = type.Published ? 1 : 0;
            portionSize.Value = type.PortionSize;
            iconId.Value = type.IconId.HasValue ? type.IconId.Value : DBNull.Value;
            mass.Value = type.Mass.HasValue ? type.Mass.Value : DBNull.Value;
            radius.Value = type.Radius.HasValue ? type.Radius.Value : DBNull.Value;
            volume.Value = type.Volume.HasValue ? type.Volume.Value : DBNull.Value;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
