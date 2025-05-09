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
        var source = File.ReadAllText(Path.Combine(Plugin.Instance.ScriptsPath, $"{executor}.mpl"));

        ProgramContext programContext = null;
        AbstractValue result = null;
        AbstractLanguageException exception = null;

        try
        {
            var lexer = new Lexer(source, executor, LexerConfiguration.Default);
            var tokens = lexer.Tokenize();

            var parser = new Parser(tokens, executor, new ParserConfiguration
            {
                LexerConfiguration = lexer.Configuration
            });
            var functionBodyExpression = parser.Parse();

            programContext = new ProgramContext(executor);
        
            ExiledKitModule.Include(Plugin.Instance, programContext, Plugin.Instance.OnEnabledListener, Plugin.Instance.OnDisabledListener);
            
            result = functionBodyExpression.Evaluate(programContext);

            var plugin = programContext.Variables.Get(null, $"{executor}_plugin", executor, Location.Default);

            var getterContext = new VariableGetterContext
            {
                ProgramContext = programContext,
                Location = Location.Default
            };

            var pluginType = (TypeValue) plugin.GetValue(getterContext);

            var listenerMember = pluginType.Get(new KeyTypeMemberIdentification { Identifier = "on_enabled" });
            var listenerType = (TypeValue) ((TypeMemberValue)listenerMember).Value;
            var listener = (Listener) listenerType.ObjectTarget;

            listener.Invoke(null, Location.Default);
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