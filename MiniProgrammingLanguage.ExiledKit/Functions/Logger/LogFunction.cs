using System;
using Discord;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Functions.Logger;

public static class LogFunction
{
    public static LanguageFunctionInstance Instance { get; } = Create();
    
    public static LanguageFunctionInstance Create()
    {
        return new LanguageFunctionInstanceBuilder()
            .SetName("log")
            .SetArguments(new FunctionArgument("content", ObjectTypeValue.String))
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetBind(Log)
            .Build();
    }

    public static AbstractValue Log(LanguageFunctionExecuteContext context)
    {
        var content = context.Arguments[0];
        var message = $"[{context.ProgramContext.ExecutorName}] {content.AsString(context.ProgramContext, context.Location)}";
        
        Exiled.API.Features.Log.Send(message, LogLevel.Info, ConsoleColor.Cyan);
        
        return VoidValue.Instance;
    }
}