using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TorneioLeft4Dead2FunctionApp.Json.Converters;

public class JsonStringOrNumberEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
            {
                var value = reader.GetString();
                if (Enum.TryParse(value, true, out T @enum))
                    return @enum;

                throw new JsonException($"Unable to convert \"{value}\" to {typeof(T).Name}");
            }
            case JsonTokenType.Number:
            {
                var value = reader.GetInt32();
                if (Enum.IsDefined(typeof(T), value))
                    return (T)Enum.ToObject(typeof(T), value);

                throw new JsonException($"Unable to convert \"{value}\" to {typeof(T).Name}");
            }
            default:
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeof(T).Name}");
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(Convert.ToInt32(value));
    }
}