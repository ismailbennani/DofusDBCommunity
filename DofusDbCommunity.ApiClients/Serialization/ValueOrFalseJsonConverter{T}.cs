using System.Text.Json;
using System.Text.Json.Serialization;
using DofusDbCommunity.Models.Common;

namespace DofusDbCommunity.ApiClients.Serialization;

public class ValueOrFalseJsonConverter<T> : JsonConverter<ValueOrFalse<T>>
{
    public override ValueOrFalse<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.False)
        {
            return new ValueOrFalse<T> { Value = default };
        }

        T? value = JsonSerializer.Deserialize<T>(ref reader, options);
        return new ValueOrFalse<T> { Value = value };
    }

    public override void Write(Utf8JsonWriter writer, ValueOrFalse<T> value, JsonSerializerOptions options)
    {
        if (value.IsFalse)
        {
            writer.WriteBooleanValue(false);
        }
        else
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}
