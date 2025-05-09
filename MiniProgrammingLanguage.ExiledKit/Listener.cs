using System;
using System.Collections.Generic;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Exceptions;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions.Interfaces;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Parser.Ast;
using MiniProgrammingLanguage.ExiledKit.Functions.Logger;

namespace MiniProgrammingLanguage.ExiledKit;

public class Listener
{
    public IReadOnlyList<KeyValuePair<ProgramContext, IFunctionInstance>> Listeners => _listeners;

    private readonly List<KeyValuePair<ProgramContext, IFunctionInstance>> _listeners = new();

    //If we unsubscribe from listener in function who was subscribed, it will throw 'collection was modified' exception
    private readonly List<int> _delete = new();

    private bool _isInvoking;

    public void Subscribe(ProgramContext programContext, IFunctionInstance functionInstance)
    {
        var member = new KeyValuePair<ProgramContext, IFunctionInstance>(programContext, functionInstance);
        
        _listeners.Add(member);
    }
    
    public void Unsubscribe(IFunctionInstance functionInstance)
    {
        var index = _listeners.FindIndex(listener => listener.Value == functionInstance);

        if (index < 0)
        {
            return;
        }

        if (_isInvoking)
        {
            _delete.Add(index);
            
            return;
        }
        
        _listeners.RemoveAt(index);
    }
    
    public void Invoke(FunctionBodyExpression root, Location location, params AbstractValue[] values)
    {
        var arguments = new ValueExpression[values.Length];

        for (var i = 0; i < values.Length; i++)
        {
            arguments[i] = new ValueExpression(values[i], location);
        }

        _isInvoking = true;
        
        foreach (var listener in _listeners)
        {
            try
            {
                listener.Value.Evaluate(new FunctionExecuteContext
                {
                    ProgramContext = listener.Key,
                    Arguments = arguments,
                    Root = root,
                    Location = location
                });
            }
            catch (AbstractLanguageException abstractLanguageException)
            {
                LogFunction.Log(listener.Key, abstractLanguageException.Message);
            }
            catch (Exception exception)
            {
                LogFunction.Log(listener.Key, $"{exception} {location}");
            }
        }

        _isInvoking = false;

        foreach (var delete in _delete)
        {
            _listeners.RemoveAt(delete);
        }
        
        _delete.Clear();
    }
}