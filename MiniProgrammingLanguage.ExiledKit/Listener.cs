using System.Collections.Generic;
using Exiled.API.Interfaces;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions.Interfaces;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Parser.Ast;

namespace MiniProgrammingLanguage.ExiledKit;

public class Listener
{
    public IReadOnlyList<KeyValuePair<ProgramContext, IFunctionInstance>> Listeners => _listeners;

    private readonly List<KeyValuePair<ProgramContext, IFunctionInstance>> _listeners = new();

    public void Subscribe(ProgramContext programContext, IFunctionInstance functionInstance)
    {
        _listeners.Add(new KeyValuePair<ProgramContext, IFunctionInstance>(programContext, functionInstance));
    }
    
    public void Unsubscribe(IFunctionInstance functionInstance)
    {
        _listeners.RemoveAt(_listeners.FindIndex(listener => listener.Value == functionInstance));
    }
    
    public void Invoke(FunctionBodyExpression root, Location location, params AbstractValue[] values)
    {
        var arguments = new ValueExpression[values.Length];

        for (var i = 0; i < values.Length; i++)
        {
            arguments[i] = new ValueExpression(values[i], location);
        }
        
        foreach (var listener in _listeners)
        {
            listener.Value.Evaluate(new FunctionExecuteContext
            {
                ProgramContext = listener.Key,
                Arguments = arguments,
                Root = root,
                Location = location
            });
        }
    }
}