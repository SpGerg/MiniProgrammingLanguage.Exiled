using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Components;

namespace MiniProgrammingLanguage.ExiledKit.Types.Unity;

public static class ComponentType
{
    public static UserTypeInstance Instance { get; } = Create();
    
    public static UserTypeInstance Create()
    {
        return new UserTypeInstanceBuilder()
            .SetName("un_component")
            .SetModule(ExiledKitModule.Name)
            .SetAccess(AccessType.ReadOnly)
            .AddMember(new TypeLanguageVariableMemberInstanceBuilder()
                .SetParent("un_component")
                .SetName("awake")
                .SetGetBind(GetAwake)
                .SetSetBind(SetAwake)
                .SetType(ObjectTypeValue.Function)
                .Build())
            .AddMember(new TypeLanguageVariableMemberInstanceBuilder()
                .SetParent("un_component")
                .SetName("update")
                .SetGetBind(GetUpdate)
                .SetSetBind(SetUpdate)
                .SetType(ObjectTypeValue.Function)
                .Build())
            .Build();
    }

    public static AbstractValue CreateByComponent(CustomComponent component)
    {
        var value = Instance.Create();
        value.ObjectTarget = component;

        return value;
    }
    
    public static AbstractValue GetAwake(TypeMemberGetterContext context)
    {
        var component = (CustomComponent) context.Type.ObjectTarget;
        return component.AwakeFunction is null ? NoneValue.Instance : component.AwakeFunction;
    }

    public static void SetAwake(TypeMemberSetterContext context)
    {
        var component = (CustomComponent) context.Type.ObjectTarget;
        component.AwakeFunction = (FunctionValue) context.Value;
    }
    
    public static AbstractValue GetUpdate(TypeMemberGetterContext context)
    {
        var component = (CustomComponent) context.Type.ObjectTarget;
        return component.UpdateFunction is null ? NoneValue.Instance : component.UpdateFunction;
    }

    public static void SetUpdate(TypeMemberSetterContext ev)
    {
        var component = (CustomComponent) ev.Type.ObjectTarget;
        component.UpdateFunction = (FunctionValue)ev.Value;
    }
}