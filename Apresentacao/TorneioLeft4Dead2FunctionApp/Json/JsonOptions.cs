using System.Text.Json;
using System.Text.Json.Serialization;
using TorneioLeft4Dead2FunctionApp.Json.Converters;

namespace TorneioLeft4Dead2FunctionApp.Json;

public static class JsonOptions
{
    public static void UseAppOptions(this JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.Converters.Add(new JsonStringOrNumberEnumConverterFactory());
    }

    public static void UseAppOptions(this Microsoft.AspNetCore.Mvc.JsonOptions options)
    {
        options.JsonSerializerOptions.UseAppOptions();
    }
}