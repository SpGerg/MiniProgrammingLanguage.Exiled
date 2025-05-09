using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using ValueType = MiniProgrammingLanguage.Core.Interpreter.Values.Enums.ValueType;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

public class SerializableNoneValue : AbstractSerializableValue
{
    public override ValueType Type { get; set; } = ValueType.None;
    

    public override AbstractValue Deserialize(ProgramContext programContext)
    {
        return NoneValue.Instance;
    }
}