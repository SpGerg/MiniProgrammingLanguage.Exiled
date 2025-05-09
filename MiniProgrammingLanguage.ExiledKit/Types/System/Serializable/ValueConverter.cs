using System;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ValueType = MiniProgrammingLanguage.Core.Interpreter.Values.Enums.ValueType;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable;

public class ValueConverter : JsonConverter
{
    private static readonly JsonSerializerSettings SpecifiedSubclassConversion = new() { ContractResolver = new ValueSpecifiedConcreteClassConverter() };

    public static ValueType[] CorrectTypes { get; } =
    {
        ValueType.String,
        ValueType.Boolean,
        ValueType.RoundNumber,
        ValueType.Number,
        ValueType.None
    };

    public static string CorrectTypesMessage { get; } = string.Join(", ", CorrectTypes);

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var typeToken = jsonObject[nameof(AbstractSerializableValue.Type)];
        var typeValue = typeToken.Value<long>();
        var type = (ValueType) typeValue;

        if (!CorrectTypes.Contains(type))
        {
            InterpreterThrowHelper.ThrowIncorrectTypeException(CorrectTypesMessage, type.ToString(), Location.Default);
        }

        return type switch
        {
            ValueType.String => JsonConvert.DeserializeObject<SerializableStringValue>(jsonObject.ToString(), SpecifiedSubclassConversion),
            ValueType.Boolean => JsonConvert.DeserializeObject<SerializableBooleanValue>(jsonObject.ToString(), SpecifiedSubclassConversion),
            ValueType.RoundNumber => JsonConvert.DeserializeObject<SerializableRoundNumberValue>(jsonObject.ToString(), SpecifiedSubclassConversion),
            ValueType.Number => JsonConvert.DeserializeObject<SerializableNumberValue>(jsonObject.ToString(), SpecifiedSubclassConversion),
            ValueType.None => JsonConvert.DeserializeObject<SerializableNoneValue>(jsonObject.ToString(), SpecifiedSubclassConversion),
            _ => throw new NotImplementedException()
        };
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(AbstractSerializableValue);
    }
}