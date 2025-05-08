using Exiled.API.Interfaces;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types.Identifications;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Types.System;

namespace MiniProgrammingLanguage.ExiledKit.Types.Exiled;

public static class PluginType
{
    public static UserTypeInstance Instance { get; } = Create();
    
    public static UserTypeInstance Create()
    {
        return new UserTypeInstanceBuilder()
            .SetName("ex_plugin")
            .SetModule("exiled-kit")
            .SetAccess(AccessType.Static)
            .AddMember(new TypeVariableMemberInstanceBuilder()
                .SetParent("ex_plugin")
                .SetName("name")
                .SetAccess(AccessType.Static | AccessType.ReadOnly)
                .SetType(ObjectTypeValue.String)
                .Build())
            .AddMember(new TypeVariableMemberInstanceBuilder()
                .SetParent("ex_plugin")
                .SetName("on_enabled")
                .SetAccess(AccessType.Static | AccessType.ReadOnly)
                .SetType(new ObjectTypeValue("listener", ValueType.Type))
                .Build())
            .AddMember(new TypeVariableMemberInstanceBuilder()
                .SetParent("ex_plugin")
                .SetName("on_disabled")
                .SetAccess(AccessType.Static | AccessType.ReadOnly)
                .SetType(new ObjectTypeValue("listener", ValueType.Type))
                .Build())
            .Build();
    }

    public static TypeValue CreateValue(IPlugin<IConfig> plugin, string name, Listener onEnabled, Listener onDisabled)
    {
        var value = Instance.Create();
        value.ObjectTarget = plugin;
        
        var nameMember = (TypeMemberValue) value.Get(new KeyTypeMemberIdentification { Identifier = "name" });
        nameMember.Value = new StringValue(name);
        
        var onEnabledMember = (TypeMemberValue) value.Get(new KeyTypeMemberIdentification { Identifier = "on_enabled" });
        onEnabledMember.Value = ListenerType.CreateValue(onEnabled);

        var onDisabledMember = (TypeMemberValue) value.Get(new KeyTypeMemberIdentification { Identifier = "on_disabled" });
        onDisabledMember.Value = ListenerType.CreateValue(onDisabled);

        return value;
    }
}