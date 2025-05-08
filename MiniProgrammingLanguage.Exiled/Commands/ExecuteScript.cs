using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using CommandSystem;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Exceptions;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Lexer;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.ExiledKit;

namespace MiniProgrammingLanguage.Exiled.Commands;

public sealed class ExecuteScript : ICommand
{
    public string Command => "execute";

    public string Description => "Execute given script";

    public string[] Aliases { get; } = Array.Empty<string>();

    private const string RaExecutor = "RA admin";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var source = arguments.Aggregate(string.Empty, (current, argument) => current + $" {argument}");
        var lexer = new Lexer(source, RaExecutor, LexerConfiguration.Default);
        var tokens = lexer.Tokenize();

        var parser = new Parser(tokens, RaExecutor, new ParserConfiguration
        {
            LexerConfiguration = lexer.Configuration
        });
        var functionBodyExpression = parser.Parse();

        AbstractValue result = null;

        var programContext = new ProgramContext(RaExecutor);
        
        ExiledKitModule.Include(Plugin.Instance, programContext, Plugin.Instance.OnEnabledListener, Plugin.Instance.OnDisabledListener);

        AbstractLanguageException exception = null;

        try
        {
            result = functionBodyExpression.Evaluate(programContext);

            while (programContext.Tasks.Entities.Any())
            {
            }
        }
        catch (AbstractLanguageException abstractLanguageException)
        {
            exception = abstractLanguageException;
        }

        if (exception is not null)
        {
            response = exception.Message;
            return false;
        }

        response = $"Script executed. Result: {(result is VoidValue or NoneValue ? result : result.AsString(programContext, Location.Default))}";
        return true;
    }
}