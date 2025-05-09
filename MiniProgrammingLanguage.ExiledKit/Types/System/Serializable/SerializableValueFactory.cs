using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.ExiledKit.Types.System.Serializable.Values;

namespace MiniProgrammingLanguage.ExiledKit.Types.System.Serializable;

public static class SerializableValueFactory
{
    public static AbstractSerializableValue Serialize(AbstractValue abstractValue, Location location)
    {
        if (!ValueConverter.CorrectTypes.Contains(abstractValue.Type))
        {
            InterpreterThrowHelper.ThrowIncorrectTypeException(ValueConverter.CorrectTypesMessage, abstractValue.Type.ToString(), location);
        }
        
        return abstractValue switch
        {
            RoundNumberValue roundNumberValue => new SerializableRoundNumberValue(roundNumberValue.Value),
            NumberValue numberValue => new SerializableNumberValue(numberValue.Value),
            StringValue stringValue => new SerializableStringValue(stringValue.Value),
            BooleanValue booleanValue => new SerializableBooleanValue(booleanValue.Value),
            _ => new SerializableNoneValue()
        };
    }
}