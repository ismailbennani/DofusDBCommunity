using System.Text.Json;
using System.Text.Json.Serialization;

namespace DofusDbCommunity.ApiClients.Serialization;

public class ValueTupleJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        Type genericTypeDefinition = typeToConvert.GetGenericTypeDefinition();
        return genericTypeDefinition == typeof(ValueTuple<>)
               || genericTypeDefinition == typeof(ValueTuple<,>)
               || genericTypeDefinition == typeof(ValueTuple<,,>)
               || genericTypeDefinition == typeof(ValueTuple<,,,>)
               || genericTypeDefinition == typeof(ValueTuple<,,,,>)
               || genericTypeDefinition == typeof(ValueTuple<,,,,,>)
               || genericTypeDefinition == typeof(ValueTuple<,,,,,,>)
               || genericTypeDefinition == typeof(ValueTuple<,,,,,,,>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type genericTypeDefinition = typeToConvert.GetGenericTypeDefinition();

        Type converterType;
        if (genericTypeDefinition == typeof(ValueTuple<>))
        {
            Type type1 = typeToConvert.GetGenericArguments()[0];
            converterType = typeof(ValueTupleJsonConverter<>).MakeGenericType(type1);
        }
        else if (genericTypeDefinition == typeof(ValueTuple<,>))
        {
            Type type1 = typeToConvert.GetGenericArguments()[0];
            Type type2 = typeToConvert.GetGenericArguments()[1];
            converterType = typeof(ValueTupleJsonConverter<,>).MakeGenericType(type1, type2);
        }
        else if (genericTypeDefinition == typeof(ValueTuple<,,>))
        {
            Type type1 = typeToConvert.GetGenericArguments()[0];
            Type type2 = typeToConvert.GetGenericArguments()[1];
            Type type3 = typeToConvert.GetGenericArguments()[2];
            converterType = typeof(ValueTupleJsonConverter<,,>).MakeGenericType(type1, type2, type3);
        }
        else if (genericTypeDefinition == typeof(ValueTuple<,,,>))
        {
            Type type1 = typeToConvert.GetGenericArguments()[0];
            Type type2 = typeToConvert.GetGenericArguments()[1];
            Type type3 = typeToConvert.GetGenericArguments()[2];
            Type type4 = typeToConvert.GetGenericArguments()[3];
            converterType = typeof(ValueTupleJsonConverter<,,,>).MakeGenericType(type1, type2, type3, type4);
        }
        else if (genericTypeDefinition == typeof(ValueTuple<,,,,>))
        {
            Type type1 = typeToConvert.GetGenericArguments()[0];
            Type type2 = typeToConvert.GetGenericArguments()[1];
            Type type3 = typeToConvert.GetGenericArguments()[2];
            Type type4 = typeToConvert.GetGenericArguments()[3];
            Type type5 = typeToConvert.GetGenericArguments()[4];
            converterType = typeof(ValueTupleJsonConverter<,,,,>).MakeGenericType(type1, type2, type3, type4, type5);
        }
        else if (genericTypeDefinition == typeof(ValueTuple<,,,,,>))
        {
            Type type1 = typeToConvert.GetGenericArguments()[0];
            Type type2 = typeToConvert.GetGenericArguments()[1];
            Type type3 = typeToConvert.GetGenericArguments()[2];
            Type type4 = typeToConvert.GetGenericArguments()[3];
            Type type5 = typeToConvert.GetGenericArguments()[4];
            Type type6 = typeToConvert.GetGenericArguments()[5];
            converterType = typeof(ValueTupleJsonConverter<,,,,,>).MakeGenericType(type1, type2, type3, type4, type5, type6);
        }
        else if (genericTypeDefinition == typeof(ValueTuple<,,,,,,>))
        {
            Type type1 = typeToConvert.GetGenericArguments()[0];
            Type type2 = typeToConvert.GetGenericArguments()[1];
            Type type3 = typeToConvert.GetGenericArguments()[2];
            Type type4 = typeToConvert.GetGenericArguments()[3];
            Type type5 = typeToConvert.GetGenericArguments()[4];
            Type type6 = typeToConvert.GetGenericArguments()[5];
            Type type7 = typeToConvert.GetGenericArguments()[6];
            converterType = typeof(ValueTupleJsonConverter<,,,,,,>).MakeGenericType(type1, type2, type3, type4, type5, type6, type7);
        }
        else
        {
            throw new InvalidOperationException($"Invalid type {typeToConvert}.");
        }

        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}
