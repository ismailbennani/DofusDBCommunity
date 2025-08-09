﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace DofusSharp.DofusDb.ApiClients.Serialization;

public class ValueTupleJsonConverter<T1> : JsonConverter<ValueTuple<T1>>
{
    public override ValueTuple<T1> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected start of array.");
        }

        reader.Read();
        T1 item1 = JsonSerializer.Deserialize<T1>(ref reader, options) ?? throw new JsonException("Could not deserialize 1st element of tuple.");

        reader.Read(); // Read the end of the array
        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Expected end of array.");
        }

        return new ValueTuple<T1>(item1);
    }

    public override void Write(Utf8JsonWriter writer, ValueTuple<T1> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, value.Item1, options);
        writer.WriteEndArray();
    }
}
