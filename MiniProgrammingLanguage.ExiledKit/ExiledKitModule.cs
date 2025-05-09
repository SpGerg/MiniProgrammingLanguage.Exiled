using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Variables;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Enums.Exiled;
using MiniProgrammingLanguage.ExiledKit.Functions.Logger;
using MiniProgrammingLanguage.ExiledKit.Functions.System;
using MiniProgrammingLanguage.ExiledKit.Functions.Unity;
using MiniProgrammingLanguage.ExiledKit.Types.Exiled;
using MiniProgrammingLanguage.ExiledKit.Types.System;
using MiniProgrammingLanguage.ExiledKit.Types.Unity;

namespace MiniProgrammingLanguage.ExiledKit;

public static class ExiledKitModule
{
    public const string Name = "exiled-kit";
    
    public static List<TypeValue> Players => _players;

    private static readonly List<TypeValue> _players = new();

    static ExiledKitModule()
    {
        Exiled.Events.Handlers.Player.Verified += InvokeOnVerified;
        Exiled.Events.Handlers.Player.Left += InvokeOnLeft;
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

        AddEventListener(programContext, "on_verified", PlayerListeners.Verified);
        AddEventListener(programContext, "on_left", PlayerListeners.Left);
        
        programContext.Enums.Add(RoleTypeEnum.Instance);
        programContext.Variables.Add(RoleTypeEnum.VariableInstance);
        
        programContext.Types.Add(PluginType.Instance);
        programContext.Types.Add(ListenerType.Instance);
        programContext.Types.Add(ComponentType.Instance);
        programContext.Types.Add(SaverType.Instance);

        programContext.Functions.Add(CreateComponentFunction.Instance);
        programContext.Functions.Add(AddComponentFunction.Instance);
        programContext.Functions.Add(SpawnFunction.Instance);
        programContext.Functions.Add(LogFunction.Instance);
    }

    private static void InvokeOnVerified(VerifiedEventArgs ev)
    {
        var player = PlayerType.CreateValue(ev.Player);
        
        _players.Add(player);

        PlayerListeners.VerifiedListener.Invoke(null, Location.Default, player);
    }
    
    private static void InvokeOnLeft(LeftEventArgs ev)
    {
        var player = PlayerType.CreateValue(ev.Player);

        _players.Remove(player);

        PlayerListeners.LeftListener.Invoke(null, Location.Default, player);
    }

    private static void AddEventListener(ProgramContext programContext, string name, AbstractValue listener)
    {
        programContext.Variables.Add(new UserVariableInstance
        {
            Name = name,
            Module = Name,
            Access = AccessType.Static | AccessType.ReadOnly,
            Type = new ObjectTypeValue("listener", ValueType.Type),
            Value = listener,
            Root = null
        });
    }
}