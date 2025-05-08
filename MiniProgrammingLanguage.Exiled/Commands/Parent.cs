using System;
using CommandSystem;
using Exiled.API.Features.Pools;

namespace MiniProgrammingLanguage.Exiled.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
public sealed class Parent : ParentCommand
{
    public Parent()
    {
        LoadGeneratedCommands();
        
        var stringBuilder = StringBuilderPool.Pool.Get(Commands.Count);
        
        stringBuilder.AppendLine(".mpl: ");
        
        foreach (var command in Commands)
        {
            stringBuilder.AppendLine($"\t- {command.Key}: {command.Value.Description}");
        }

        _message = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
    }

    public override string Command => "mpl";

    public override string Description => "MPL commands";

    public override string[] Aliases { get; } = Array.Empty<string>();

    private readonly string _message;
    
    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new ExecuteScript());
        RegisterCommand(new Run());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = _message;
        return true;
    }
}