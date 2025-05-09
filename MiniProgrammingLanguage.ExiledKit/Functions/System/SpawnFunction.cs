using System;
using Exiled.API.Features;
using MEC;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Functions.Logger;

namespace MiniProgrammingLanguage.ExiledKit.Functions.System;

public static class SpawnFunction
{
    public static LanguageFunctionInstance Instance { get; } = Create();
    
    public static LanguageFunctionInstance Create()
    {
        return new LanguageFunctionInstanceBuilder()
            .SetName("spawn")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetArguments(new FunctionArgument("delay", ObjectTypeValue.Number), new FunctionArgument("function", ObjectTypeValue.Function))
            .SetBind(Spawn)
            .Build();
    }

    public static AbstractValue Spawn(LanguageFunctionExecuteContext context)
    {
        var delay = context.Arguments[0].AsNumber(context.ProgramContext, context.Location);
        var function = (FunctionValue) context.Arguments[1];

        var arguments = new AbstractEvaluableExpression[context.Arguments.Length];

        for (var i = 0; i < context.Arguments.Length; i++)
        {
            arguments[i] = new ValueExpression(context.Arguments[i], context.Location);
        }

        var functionContext = new FunctionExecuteContext
        {
            ProgramContext = context.ProgramContext,
            Arguments = arguments,
            Root = context.Root,
            Location = context.Location
        };

        Timing.CallDelayed(delay, () =>
        {
            try
            {
                function.Value.Evaluate(functionContext);
            }
            catch (Exception exception)
            {
                LogFunction.Log(context.ProgramContext, exception.Message);
            }
        });
        
        return VoidValue.Instance;
    }
}