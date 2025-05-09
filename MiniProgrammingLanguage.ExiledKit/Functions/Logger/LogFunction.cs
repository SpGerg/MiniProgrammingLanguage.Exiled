using System;
using Discord;
using MiniProgrammingLanguage.Core.Interpreter;
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
        Log(context.ProgramContext, content.AsString(context.ProgramContext, context.Location));
        
        return VoidValue.Instance;
    }
    
    public static void Log(ProgramContext programContext, string content)
    {
        var message = $"[{programContext.ExecutorName}] {content}";
        
        Exiled.API.Features.Log.Send(message, LogLevel.Info, ConsoleColor.Cyan);
    }
}