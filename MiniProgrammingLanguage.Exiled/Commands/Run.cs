using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Exceptions;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types.Identifications;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Variables;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Lexer;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast;
using MiniProgrammingLanguage.Core.Parser.Exceptions;
using MiniProgrammingLanguage.ExiledKit;

namespace MiniProgrammingLanguage.Exiled.Commands;

public sealed class Run : ICommand
{
    public string Command => "run";

    public string Description => "Run script file";

    public string[] Aliases { get; } = Array.Empty<string>();

    private readonly ExecuteScript _executeScript = new();

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 1)
        {
            response = "Except filename";
            return false;
        }

        var executor = arguments.At(0);
        string source;

        if (executor.EndsWith(".mpl"))
        {
            source = File.ReadAllText(Path.Combine(Plugin.Instance.ScriptsPath, executor));
        }
        else
        {
            var path = Path.Combine(Plugin.Instance.ScriptsPath, executor);

            if (!Directory.Exists(path))
            {
                response = $"Project with {executor} name not found";
                return false;
            }

            var root = Path.Combine(path, "root.mpl");
            
            if (!File.Exists(root))
            {
                response = $"Root file in project with {executor} name not found";
                return false;
            }
            
            source = File.ReadAllText(root);
        }

        var result = Script.Run(source, executor, out var programContext, out var exception);

        if (exception is not null)
        {
            response = $"Script execution error: {exception.Message}";
            return true;
        }

        response = $"Script executed. Result: {(result is VoidValue or NoneValue ? result : result.AsString(programContext, Location.Default))}";
        return true;
    }
}