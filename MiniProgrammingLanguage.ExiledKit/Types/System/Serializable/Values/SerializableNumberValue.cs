using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

public class SerializableNumberValue : AbstractSerializableValue
{
    public SerializableNumberValue() {}
        
    public SerializableNumberValue(float value)
    {
        Value = value;
    }
    
    public override ValueType Type { get; set; } = ValueType.Number;
    
    public float Value { get; set; }
    
    public override AbstractValue Deserialize(ProgramContext programContext)
    {
        return new NumberValue(Value);
    }
}