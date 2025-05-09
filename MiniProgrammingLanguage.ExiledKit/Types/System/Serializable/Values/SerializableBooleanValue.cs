using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

public class SerializableBooleanValue : AbstractSerializableValue
{
    public SerializableBooleanValue() {}
    
    public SerializableBooleanValue(bool value)
    {
        Value = value;
    }

    public override ValueType Type { get; set; } = ValueType.Array;
    
    public bool Value { get; set; }
    
    public override AbstractValue Deserialize(ProgramContext programContext)
    {
        return new BooleanValue(Value);
    }
}