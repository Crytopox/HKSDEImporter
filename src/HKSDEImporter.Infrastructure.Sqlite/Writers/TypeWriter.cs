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
            "INSERT INTO invTypes (typeID, groupID, typeName, description, mass, volume, capacity, portionSize, raceID, basePrice, published, marketGroupID, iconID, soundID, graphicID, radius) " +
            "VALUES ($typeId, $groupId, $typeName, $description, $mass, $volume, $capacity, $portionSize, $raceId, $basePrice, $published, $marketGroupId, $iconId, $soundId, $graphicId, $radius);";
        command.Transaction = transaction;

        var typeId = command.CreateParameter(); typeId.ParameterName = "$typeId"; command.Parameters.Add(typeId);
        var groupId = command.CreateParameter(); groupId.ParameterName = "$groupId"; command.Parameters.Add(groupId);
        var typeName = command.CreateParameter(); typeName.ParameterName = "$typeName"; command.Parameters.Add(typeName);
        var description = command.CreateParameter(); description.ParameterName = "$description"; command.Parameters.Add(description);
        var mass = command.CreateParameter(); mass.ParameterName = "$mass"; command.Parameters.Add(mass);
        var volume = command.CreateParameter(); volume.ParameterName = "$volume"; command.Parameters.Add(volume);
        var capacity = command.CreateParameter(); capacity.ParameterName = "$capacity"; command.Parameters.Add(capacity);
        var portionSize = command.CreateParameter(); portionSize.ParameterName = "$portionSize"; command.Parameters.Add(portionSize);
        var raceId = command.CreateParameter(); raceId.ParameterName = "$raceId"; command.Parameters.Add(raceId);
        var basePrice = command.CreateParameter(); basePrice.ParameterName = "$basePrice"; command.Parameters.Add(basePrice);
        var published = command.CreateParameter(); published.ParameterName = "$published"; command.Parameters.Add(published);
        var marketGroupId = command.CreateParameter(); marketGroupId.ParameterName = "$marketGroupId"; command.Parameters.Add(marketGroupId);
        var iconId = command.CreateParameter(); iconId.ParameterName = "$iconId"; command.Parameters.Add(iconId);
        var soundId = command.CreateParameter(); soundId.ParameterName = "$soundId"; command.Parameters.Add(soundId);
        var graphicId = command.CreateParameter(); graphicId.ParameterName = "$graphicId"; command.Parameters.Add(graphicId);
        var radius = command.CreateParameter(); radius.ParameterName = "$radius"; command.Parameters.Add(radius);

        foreach (var type in types)
        {
            cancellationToken.ThrowIfCancellationRequested();

            typeId.Value = type.TypeId;
            groupId.Value = type.GroupId;
            typeName.Value = type.Name;
            description.Value = string.IsNullOrWhiteSpace(type.Description) ? DBNull.Value : type.Description;
            mass.Value = type.Mass.HasValue ? type.Mass.Value : DBNull.Value;
            volume.Value = type.Volume.HasValue ? type.Volume.Value : DBNull.Value;
            capacity.Value = type.Capacity.HasValue ? type.Capacity.Value : DBNull.Value;
            portionSize.Value = type.PortionSize;
            raceId.Value = type.RaceId.HasValue ? type.RaceId.Value : DBNull.Value;
            basePrice.Value = type.BasePrice.HasValue ? type.BasePrice.Value : DBNull.Value;
            published.Value = type.Published ? 1 : 0;
            marketGroupId.Value = type.MarketGroupId.HasValue ? type.MarketGroupId.Value : DBNull.Value;
            iconId.Value = type.IconId.HasValue ? type.IconId.Value : DBNull.Value;
            soundId.Value = type.SoundId.HasValue ? type.SoundId.Value : DBNull.Value;
            graphicId.Value = type.GraphicId.HasValue ? type.GraphicId.Value : DBNull.Value;
            radius.Value = type.Radius.HasValue ? type.Radius.Value : DBNull.Value;

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
