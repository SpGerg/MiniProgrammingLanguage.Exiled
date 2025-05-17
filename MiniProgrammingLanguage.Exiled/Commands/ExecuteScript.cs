using System;
using System.Linq;
using CommandSystem;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter.Values;

namespace MiniProgrammingLanguage.Exiled.Commands;

public sealed class ExecuteScript : ICommand
{
    public string Command => "execute";

    public string Description => "Execute given script";

    public string[] Aliases { get; } = Array.Empty<string>();

    private const string RaExecutor = "ra";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var source = arguments.Aggregate(string.Empty, (current, argument) => current + $" {argument}");
        var result = Script.Run(source, RaExecutor, out var programContext, out var exception);

        if (exception is not null)
        {
            response = $"Script execution error: {exception.Message}";
            return true;
        }

        response = $"Script executed. Result: {(result is VoidValue or NoneValue ? result : result.AsString(programContext, Location.Default))}";
        return true;
    }
}