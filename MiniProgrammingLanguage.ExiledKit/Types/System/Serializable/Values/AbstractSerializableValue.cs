using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using Newtonsoft.Json;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

[JsonConverter(typeof(ValueConverter))]
public abstract class AbstractSerializableValue
{
    public abstract ValueType Type { get; set; }
    
    public abstract AbstractValue Deserialize(ProgramContext programContext);
}