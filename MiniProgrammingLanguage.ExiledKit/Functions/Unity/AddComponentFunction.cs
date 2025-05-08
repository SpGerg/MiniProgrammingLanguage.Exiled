using System;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Components;
using MiniProgrammingLanguage.ExiledKit.Types.Unity;
using UnityEngine;
using ValueType = MiniProgrammingLanguage.Core.Interpreter.Values.Enums.ValueType;

namespace MiniProgrammingLanguage.ExiledKit.Functions.Unity;

public static class AddComponentFunction
{
    public static LanguageFunctionInstance Instance { get; } = Create();
    
    public static LanguageFunctionInstance Create()
    {
        return new LanguageFunctionInstanceBuilder()
            .SetName("add_component")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetBind(AddComponent)
            .SetArguments(
                new FunctionArgument("game_object", new ObjectTypeValue("un_game_object", ValueType.CSharpObject)),
                new FunctionArgument("component", new ObjectTypeValue(ComponentType.Instance.Name, ValueType.Type)))
            .SetReturn(new ObjectTypeValue(ComponentType.Instance.Name, ValueType.Type))
            .Build();
    }

    public static AbstractValue AddComponent(LanguageFunctionExecuteContext context)
    {
        var gameObject = (CSharpObjectValue) context.Arguments[0];

        if (gameObject.Value is not GameObject gameObjectValue)
        {
            var name = gameObject.Value switch
            {
                null => NoneValue.Instance.ToString(),
                AbstractValue abstractValue => abstractValue.ToString(),
                _ => gameObject.Value.GetType().Name
            };
            
            InterpreterThrowHelper.ThrowIncorrectTypeException("game_object", name, context.Location);

            return null;
        }

        var type = (TypeValue) context.Arguments[1];
        var component = (CustomComponent) type.ObjectTarget;

        var instance = gameObjectValue.AddComponent<CustomComponent>();
        instance.Initialize(component.Name, component.ProgramContext, component.Root, component.Location);

        instance.AwakeFunction = component.AwakeFunction;
        instance.UpdateFunction = component.UpdateFunction;

        var functionContext = new FunctionExecuteContext
        {
            ProgramContext = component.ProgramContext,
            Arguments = Array.Empty<AbstractEvaluableExpression>(),
            Root = context.Root,
            Location = context.Location
        };
        
        component.AwakeFunction?.Value.Evaluate(functionContext);

        return ComponentType.CreateByComponent(component);
    }
}