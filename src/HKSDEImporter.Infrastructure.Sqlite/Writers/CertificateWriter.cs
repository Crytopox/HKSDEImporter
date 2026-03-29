using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class CertificateWriter : ICertificateWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public CertificateWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<Cert> certs, IReadOnlyCollection<CertMastery> masteries, IReadOnlyCollection<CertSkill> skills, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using (var certCommand = connection.CreateCommand())
        {
            certCommand.Transaction = transaction;
            certCommand.CommandText = "INSERT INTO certCerts (certID, description, groupID, name) VALUES ($certID, $description, $groupID, $name);";

            var certId = certCommand.CreateParameter(); certId.ParameterName = "$certID"; certCommand.Parameters.Add(certId);
            var description = certCommand.CreateParameter(); description.ParameterName = "$description"; certCommand.Parameters.Add(description);
            var groupId = certCommand.CreateParameter(); groupId.ParameterName = "$groupID"; certCommand.Parameters.Add(groupId);
            var name = certCommand.CreateParameter(); name.ParameterName = "$name"; certCommand.Parameters.Add(name);

            foreach (var row in certs)
            {
                certId.Value = row.CertId;
                description.Value = string.IsNullOrWhiteSpace(row.Description) ? DBNull.Value : row.Description;
                groupId.Value = row.GroupId.HasValue ? row.GroupId.Value : DBNull.Value;
                name.Value = string.IsNullOrWhiteSpace(row.Name) ? DBNull.Value : row.Name;
                await certCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var masteryCommand = connection.CreateCommand())
        {
            masteryCommand.Transaction = transaction;
            masteryCommand.CommandText = "INSERT INTO certMasteries (typeID, masteryLevel, certID) VALUES ($typeID, $masteryLevel, $certID);";

            var typeId = masteryCommand.CreateParameter(); typeId.ParameterName = "$typeID"; masteryCommand.Parameters.Add(typeId);
            var masteryLevel = masteryCommand.CreateParameter(); masteryLevel.ParameterName = "$masteryLevel"; masteryCommand.Parameters.Add(masteryLevel);
            var certId = masteryCommand.CreateParameter(); certId.ParameterName = "$certID"; masteryCommand.Parameters.Add(certId);

            foreach (var row in masteries)
            {
                typeId.Value = row.TypeId;
                masteryLevel.Value = row.MasteryLevel;
                certId.Value = row.CertId;
                await masteryCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        using (var skillCommand = connection.CreateCommand())
        {
            skillCommand.Transaction = transaction;
            skillCommand.CommandText = "INSERT INTO certSkills (certID, skillID, certLevelInt, skillLevel, certLevelText) VALUES ($certID, $skillID, $certLevelInt, $skillLevel, $certLevelText);";

            var certId = skillCommand.CreateParameter(); certId.ParameterName = "$certID"; skillCommand.Parameters.Add(certId);
            var skillId = skillCommand.CreateParameter(); skillId.ParameterName = "$skillID"; skillCommand.Parameters.Add(skillId);
            var certLevelInt = skillCommand.CreateParameter(); certLevelInt.ParameterName = "$certLevelInt"; skillCommand.Parameters.Add(certLevelInt);
            var skillLevel = skillCommand.CreateParameter(); skillLevel.ParameterName = "$skillLevel"; skillCommand.Parameters.Add(skillLevel);
            var certLevelText = skillCommand.CreateParameter(); certLevelText.ParameterName = "$certLevelText"; skillCommand.Parameters.Add(certLevelText);

            foreach (var row in skills)
            {
                certId.Value = row.CertId;
                skillId.Value = row.SkillId;
                certLevelInt.Value = row.CertLevelInt;
                skillLevel.Value = row.SkillLevel;
                certLevelText.Value = row.CertLevelText;
                await skillCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        transaction.Commit();
    }
}
