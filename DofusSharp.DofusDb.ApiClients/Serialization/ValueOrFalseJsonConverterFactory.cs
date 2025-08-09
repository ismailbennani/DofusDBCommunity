using System.Text.Json;
using System.Text.Json.Serialization;
using DofusSharp.DofusDb.Models.Common;

namespace DofusSharp.DofusDb.ApiClients.Serialization;

public class ValueOrFalseJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ValueOrFalse<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type underlyingType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(ValueOrFalseJsonConverter<>).MakeGenericType(underlyingType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}
