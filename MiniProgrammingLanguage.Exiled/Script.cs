using System.Linq;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Exceptions;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types.Identifications;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Variables;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Lexer;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.ExiledKit;

namespace MiniProgrammingLanguage.Exiled;

public static class Script
{
    public static AbstractValue Run(string source, string executor, out ProgramContext programContext, out AbstractLanguageException exception)
    {
        AbstractValue result;
        
        try
        {
            var lexer = new Lexer(source, executor, LexerConfiguration.Default);
            var tokens = lexer.Tokenize();

            var parser = new Parser(tokens, executor, new ParserConfiguration
            {
                LexerConfiguration = lexer.Configuration
            });
            var functionBodyExpression = parser.Parse();

            programContext = new ProgramContext(executor.Replace(".mpl", string.Empty));
        
            ExiledKitModule.Include(Plugin.Instance, programContext, Plugin.Instance.OnEnabledListener, Plugin.Instance.OnDisabledListener);
            
            result = functionBodyExpression.Evaluate(programContext);
            exception = null;
            
            InvokeOnEnabled(programContext);
            
            Plugin.Scripts.Add(programContext);
        }
        catch (AbstractLanguageException abstractLanguageException)
        {
            exception = abstractLanguageException;
            result = null;
            programContext = null;
        }

        return result;
    }

    public static void Stop(string executor)
    {
        var programContext = Plugin.Scripts.FirstOrDefault(entity => entity.ExecutorName == executor);
        
        if (programContext is null)
        {
            return;
        }
        
        var plugin = programContext.Variables.Get(null, $"{programContext.ExecutorName}_plugin", programContext.ExecutorName, Location.Default);
        var getterContext = new VariableGetterContext
        {
            ProgramContext = programContext,
            Location = Location.Default
        };

        Stop((TypeValue) plugin.GetValue(getterContext));
    }
    
    public static void Stop(TypeValue typeValue)
    {
        var onDisabled = typeValue.Get(new KeyTypeMemberIdentification { Identifier = "on_disabled" });
        var disabledType = (TypeValue) ((TypeMemberValue) onDisabled).Value;
        var disabled = (Listener) disabledType.ObjectTarget;

        disabled.Invoke(null, Location.Default);
    }
    
    private static void InvokeOnEnabled(ProgramContext programContext)
    {
        var plugin = programContext.Variables.Get(null, $"{programContext.ExecutorName}_plugin", programContext.ExecutorName, Location.Default);
        var getterContext = new VariableGetterContext
        {
            ProgramContext = programContext,
            Location = Location.Default
        };

        var pluginType = (TypeValue) plugin.GetValue(getterContext);

        var exists = Plugin.Scripts.FirstOrDefault(entity => entity.ExecutorName == programContext.ExecutorName);
        
        if (exists is not null)
        {
            Stop(pluginType);
        }
        
        var listenerMember = pluginType.Get(new KeyTypeMemberIdentification { Identifier = "on_enabled" });
        var listenerType = (TypeValue) ((TypeMemberValue) listenerMember).Value;
        var listener = (Listener) listenerType.ObjectTarget;

        listener.Invoke(null, Location.Default);
    }
}