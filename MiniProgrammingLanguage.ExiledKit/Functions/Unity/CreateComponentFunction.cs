using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Components;
using MiniProgrammingLanguage.ExiledKit.Types.Unity;
using UnityEngine;

namespace MiniProgrammingLanguage.ExiledKit.Functions.Unity;

public static class CreateComponentFunction
{
    public static LanguageFunctionInstance Instance { get; } = Create();
    
    public static LanguageFunctionInstance Create()
    {
        return new LanguageFunctionInstanceBuilder()
            .SetName("create_component")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetBind(CreateComponent)
            .SetArguments(new FunctionArgument("name", ObjectTypeValue.String))
            .SetReturn(new ObjectTypeValue(ComponentType.Instance.Name, ValueType.Type))
            .Build();
    }

    public static AbstractValue CreateComponent(LanguageFunctionExecuteContext context)
    {
        var name = context.Arguments[0].AsString(context.ProgramContext, context.Location);

        //We need just like storage for functions before adding it on game object
        var component = new CustomComponent();
        component.Initialize(name, context.ProgramContext, context.Root, context.Location);
        
        return ComponentType.CreateByComponent(component);
    }
}