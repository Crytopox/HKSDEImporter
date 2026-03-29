using HKSDEImporter.Core.Contracts;
using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Infrastructure.Sqlite.Connections;

namespace HKSDEImporter.Infrastructure.Sqlite.Writers;

public sealed class TranslationLanguageWriter : ITranslationLanguageWriter
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public TranslationLanguageWriter(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task WriteAsync(string outputPath, IReadOnlyCollection<TranslationLanguage> languages, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.Create(Path.GetFullPath(outputPath));
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "INSERT INTO trnTranslationLanguages (numericLanguageID, languageID, languageName) VALUES ($numericLanguageID, $languageID, $languageName);";

        var numericLanguageId = command.CreateParameter(); numericLanguageId.ParameterName = "$numericLanguageID"; command.Parameters.Add(numericLanguageId);
        var languageId = command.CreateParameter(); languageId.ParameterName = "$languageID"; command.Parameters.Add(languageId);
        var languageName = command.CreateParameter(); languageName.ParameterName = "$languageName"; command.Parameters.Add(languageName);

        foreach (var row in languages)
        {
            numericLanguageId.Value = row.NumericLanguageId;
            languageId.Value = row.LanguageId;
            languageName.Value = string.IsNullOrWhiteSpace(row.LanguageName) ? DBNull.Value : row.LanguageName;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
    }
}
