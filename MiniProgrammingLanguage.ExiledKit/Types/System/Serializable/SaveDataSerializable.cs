using System;
using System.Collections.Generic;
using MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable;

[Serializable]
public class SaveDataSerializable
{
    public Dictionary<string, AbstractSerializableValue> Values { get; set; } = new();
}