using System;
using Exiled.API.Features;
using MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ValueType = MiniProgrammingLanguage.Core.Interpreter.Values.Enums.ValueType;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable;

public class ValueConverter : JsonConverter
{
    private static readonly JsonSerializerSettings SpecifiedSubclassConversion = new() { ContractResolver = new ValueSpecifiedConcreteClassConverter() };

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

        return type switch
        {
            ValueType.RoundNumber => JsonConvert.DeserializeObject<SerializableNumberValue>(jsonObject.ToString(), SpecifiedSubclassConversion),
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