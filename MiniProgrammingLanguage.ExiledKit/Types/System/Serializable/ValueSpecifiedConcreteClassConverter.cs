using System;
using MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable;

public class ValueSpecifiedConcreteClassConverter : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(AbstractSerializableValue).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null;
        
        return base.ResolveContractConverter(objectType);
    }
}