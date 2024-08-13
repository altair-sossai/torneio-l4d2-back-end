using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TorneioLeft4Dead2FunctionApp.Json.Converters;

public class JsonStringOrNumberEnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(JsonStringOrNumberEnumConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}