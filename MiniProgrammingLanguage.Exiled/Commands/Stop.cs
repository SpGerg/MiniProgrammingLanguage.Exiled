using System;
using CommandSystem;

namespace MiniProgrammingLanguage.Exiled.Commands;

public class Stop : ICommand
{
    public string Command => "stop";

    public string Description => "Stop script";

    public string[] Aliases { get; } = Array.Empty<string>();

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 1)
        {
            response = "Except script name";
            return false;
        }
        
        Script.Stop(arguments.At(0));

        response = "Script was stopped";
        return true;
    }
}