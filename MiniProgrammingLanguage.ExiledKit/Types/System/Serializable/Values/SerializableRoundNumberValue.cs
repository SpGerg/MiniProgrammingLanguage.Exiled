using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

public class SerializableRoundNumberValue : AbstractSerializableValue
{
    public SerializableRoundNumberValue() {}
        
    public SerializableRoundNumberValue(int value)
    {
        Value = value;
    }
    
    public override ValueType Type { get; set; } = ValueType.RoundNumber;
    
    public int Value { get; set; }
    
    public override AbstractValue Deserialize(ProgramContext programContext)
    {
        return new RoundNumberValue(Value);
    }
}