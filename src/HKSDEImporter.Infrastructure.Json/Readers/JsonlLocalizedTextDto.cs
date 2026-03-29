using System.Text.Json;
using System.Text.Json.Serialization;

namespace HKSDEImporter.Infrastructure.Json.Readers;

[JsonConverter(typeof(JsonlLocalizedTextConverter))]
internal sealed class JsonlLocalizedTextDto
{
    [JsonPropertyName("en")]
    public string? En { get; init; }
}

internal sealed class JsonlLocalizedTextConverter : JsonConverter<JsonlLocalizedTextDto>
{
    public override JsonlLocalizedTextDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return new JsonlLocalizedTextDto { En = reader.GetString() };
        }

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"Unsupported localized text token: {reader.TokenType}");
        }

        string? en = null;
        string? fallback = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            var prop = reader.GetString();
            reader.Read();

            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (string.Equals(prop, "en", StringComparison.OrdinalIgnoreCase))
                {
                    en = value;
                }
                else if (fallback is null)
                {
                    fallback = value;
                }
            }
            else
            {
                using var _ = JsonDocument.ParseValue(ref reader);
            }
        }

        return new JsonlLocalizedTextDto { En = en ?? fallback };
    }

    public override void Write(Utf8JsonWriter writer, JsonlLocalizedTextDto value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.En is not null)
        {
            writer.WriteString("en", value.En);
        }

        writer.WriteEndObject();
    }
}
