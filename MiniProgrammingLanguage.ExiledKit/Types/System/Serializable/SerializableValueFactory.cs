using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable;

public static class SerializableValueFactory
{
    public static AbstractSerializableValue Serialize(AbstractValue abstractValue)
    {
        return abstractValue switch
        {
            RoundNumberValue roundNumberValue => new SerializableRoundNumberValue(roundNumberValue.Value),
            NumberValue numberValue => new SerializableNumberValue(numberValue.Value),
            _ => new SerializableNoneValue()
        };
    }
}