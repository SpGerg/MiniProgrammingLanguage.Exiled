using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

public class SerializableStringValue : AbstractSerializableValue
{
    public SerializableStringValue() {}
    
    public SerializableStringValue(string value)
    {
        Value = value;
    }

    public override ValueType Type { get; set; } = ValueType.String;
    
    public string Value { get; set; }
    
    public override AbstractValue Deserialize(ProgramContext programContext)
    {
        return new StringValue(Value);
    }
}