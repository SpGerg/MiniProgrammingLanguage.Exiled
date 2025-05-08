using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Types.System;

public static class ListenerType
{
    public static UserTypeInstance Instance { get; } = Create();
    
    public static UserTypeInstance Create()
    {
        return new UserTypeInstanceBuilder()
            .SetName("listener")
            .SetAccess(AccessType.Static)
            .SetMembers(
                new TypeLanguageFunctionMemberInstanceBuilder()
                    .SetParent("listener")
                    .SetName("subscribe")
                    .SetBind(Subscribe)
                    .SetArguments(new FunctionArgument("subscriber", ObjectTypeValue.Function))
                    .SetAccess(AccessType.Static | AccessType.ReadOnly)
                    .Build(),
                new TypeLanguageFunctionMemberInstanceBuilder()
                    .SetParent("listener")
                    .SetName("unsubscribe")
                    .SetBind(Unsubscribe)
                    .SetArguments(new FunctionArgument("unsubscriber", ObjectTypeValue.Function))
                    .SetAccess(AccessType.Static | AccessType.ReadOnly)
                    .Build(),
                new TypeLanguageFunctionMemberInstanceBuilder()
                    .SetParent("listener")
                    .SetName("invoke")
                    .SetBind(Invoke)
                    .SetArguments(new FunctionArgument("arguments", ObjectTypeValue.Array))
                    .SetAccess(AccessType.Static | AccessType.ReadOnly)
                    .Build())
            .Build();
    }

    public static TypeValue CreateValue(Listener objectTarget)
    {
        var listener = Instance.Create();
        listener.ObjectTarget = objectTarget ?? new Listener();

        return listener;
    }

    public static AbstractValue Subscribe(TypeFunctionExecuteContext context)
    {
        var listener = (Listener) context.Type.ObjectTarget;
        var argument = context.Arguments[0];

        var function = (FunctionValue) argument.Evaluate(context.ProgramContext);
        
        listener.Subscribe(context.ProgramContext, function.Value);
        
        return VoidValue.Instance;
    }
    
    public static AbstractValue Unsubscribe(TypeFunctionExecuteContext context)
    {
        var listener = (Listener) context.Type.ObjectTarget;
        var argument = context.Arguments[0];

        var function = (FunctionValue) argument.Evaluate(context.ProgramContext);
        
        listener.Unsubscribe(function.Value);
        
        return VoidValue.Instance;
    }
    
    public static AbstractValue Invoke(TypeFunctionExecuteContext context)
    {
        var listener = (Listener) context.Type.ObjectTarget;
        var argument = context.Arguments[0];

        var arguments = (ArrayValue) argument.Evaluate(context.ProgramContext);
        
        listener.Invoke(context.Root, context.Location, arguments.Value);

        return VoidValue.Instance;
    }
}