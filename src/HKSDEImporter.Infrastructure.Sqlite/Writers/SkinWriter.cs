using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class SkinWriter : ISkinWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SkinWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<Skin> skins, IReadOnlyCollection<SkinMaterial> materials, IReadOnlyCollection<SkinShip> skinShips, IReadOnlyCollection<SkinLicense> licenses, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using (var skinCommand = connection.CreateCommand())
        {
            skinCommand.Transaction = transaction;
            skinCommand.CommandText = "INSERT INTO skins (skinID, internalName, skinMaterialID) VALUES ($skinID, $internalName, $skinMaterialID);";

            var skinId = skinCommand.CreateParameter(); skinId.ParameterName = "$skinID"; skinCommand.Parameters.Add(skinId);
            var internalName = skinCommand.CreateParameter(); internalName.ParameterName = "$internalName"; skinCommand.Parameters.Add(internalName);
            var skinMaterialId = skinCommand.CreateParameter(); skinMaterialId.ParameterName = "$skinMaterialID"; skinCommand.Parameters.Add(skinMaterialId);

            foreach (var row in skins)
            {
                skinId.Value = row.SkinId;
                internalName.Value = string.IsNullOrWhiteSpace(row.InternalName) ? DBNull.Value : row.InternalName;
                skinMaterialId.Value = row.SkinMaterialId.HasValue ? row.SkinMaterialId.Value : DBNull.Value;
                await skinCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var materialCommand = connection.CreateCommand())
        {
            materialCommand.Transaction = transaction;
            materialCommand.CommandText = "INSERT INTO skinMaterials (skinMaterialID, displayNameID, materialSetID) VALUES ($skinMaterialID, $displayNameID, $materialSetID);";

            var skinMaterialId = materialCommand.CreateParameter(); skinMaterialId.ParameterName = "$skinMaterialID"; materialCommand.Parameters.Add(skinMaterialId);
            var displayNameId = materialCommand.CreateParameter(); displayNameId.ParameterName = "$displayNameID"; materialCommand.Parameters.Add(displayNameId);
            var materialSetId = materialCommand.CreateParameter(); materialSetId.ParameterName = "$materialSetID"; materialCommand.Parameters.Add(materialSetId);

            foreach (var row in materials)
            {
                skinMaterialId.Value = row.SkinMaterialId;
                displayNameId.Value = row.DisplayNameId.HasValue ? row.DisplayNameId.Value : DBNull.Value;
                materialSetId.Value = row.MaterialSetId.HasValue ? row.MaterialSetId.Value : DBNull.Value;
                await materialCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var shipCommand = connection.CreateCommand())
        {
            shipCommand.Transaction = transaction;
            shipCommand.CommandText = "INSERT INTO skinShip (skinID, typeID) VALUES ($skinID, $typeID);";

            var skinId = shipCommand.CreateParameter(); skinId.ParameterName = "$skinID"; shipCommand.Parameters.Add(skinId);
            var typeId = shipCommand.CreateParameter(); typeId.ParameterName = "$typeID"; shipCommand.Parameters.Add(typeId);

            foreach (var row in skinShips)
            {
                skinId.Value = row.SkinId;
                typeId.Value = row.TypeId;
                await shipCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var licenseCommand = connection.CreateCommand())
        {
            licenseCommand.Transaction = transaction;
            licenseCommand.CommandText = "INSERT INTO skinLicense (licenseTypeID, duration, skinID) VALUES ($licenseTypeID, $duration, $skinID);";

            var licenseTypeId = licenseCommand.CreateParameter(); licenseTypeId.ParameterName = "$licenseTypeID"; licenseCommand.Parameters.Add(licenseTypeId);
            var duration = licenseCommand.CreateParameter(); duration.ParameterName = "$duration"; licenseCommand.Parameters.Add(duration);
            var skinId = licenseCommand.CreateParameter(); skinId.ParameterName = "$skinID"; licenseCommand.Parameters.Add(skinId);

            foreach (var row in licenses)
            {
                licenseTypeId.Value = row.LicenseTypeId;
                duration.Value = row.Duration.HasValue ? row.Duration.Value : DBNull.Value;
                skinId.Value = row.SkinId.HasValue ? row.SkinId.Value : DBNull.Value;
                await licenseCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        transaction.Commit();
    }
}
