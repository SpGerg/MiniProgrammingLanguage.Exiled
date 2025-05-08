using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Variables;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Enums.Exiled;
using MiniProgrammingLanguage.ExiledKit.Functions.Logger;
using MiniProgrammingLanguage.ExiledKit.Functions.System;
using MiniProgrammingLanguage.ExiledKit.Types.Exiled;
using MiniProgrammingLanguage.ExiledKit.Types.System;

namespace MiniProgrammingLanguage.ExiledKit;

public static class ExiledKitModule
{
    static ExiledKitModule()
    {
        Exiled.Events.Handlers.Player.Verified += InvokeOnVerified;
    }
    
    public static void Include(IPlugin<IConfig> plugin, ProgramContext programContext, Listener onEnabled, Listener onDisabled)
    {
        var pluginValue = PluginType.CreateValue(plugin, programContext.ExecutorName, onEnabled, onDisabled);
        
        programContext.Variables.Add(new UserVariableInstance
        {
            Name = "plugin",
            Module = "global",
            Access = AccessType.Static | AccessType.ReadOnly,
            Type = new ObjectTypeValue("ex_plugin", ValueType.Type),
            Value = pluginValue,
            Root = null
        });
        
        programContext.Variables.Add(new UserVariableInstance
        {
            Name = "on_verified",
            Module = "exiled-kit",
            Access = AccessType.Static | AccessType.ReadOnly,
            Type = new ObjectTypeValue("listener", ValueType.Type),
            Value = PlayerListeners.Verified,
            Root = null
        });
        
        programContext.Enums.Add(RoleTypeEnum.Instance);
        programContext.Variables.Add(RoleTypeEnum.VariableInstance);
        
        programContext.Types.Add(PluginType.Instance);
        programContext.Types.Add(ListenerType.Instance);

        programContext.Functions.Add(SpawnFunction.Instance);
        programContext.Functions.Add(LogFunction.Instance);
    }

    private static void InvokeOnVerified(VerifiedEventArgs ev)
    {
        var player = PlayerType.CreateValue(ev.Player);

        PlayerListeners.VerifiedListener.Invoke(null, Location.Default, player);
    }
}