using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Exiled.Commands;
using MiniProgrammingLanguage.ExiledKit;
using MiniProgrammingLanguage.ExiledKit.Interfaces;

namespace MiniProgrammingLanguage.Exiled;

public class Plugin : Plugin<Config>, IExiledKitPlugin<Config>
{
    public static Plugin Instance { get; private set; }

    public static List<ProgramContext> Scripts { get; private set; } = new();

    public Listener OnEnabledListener { get; } = new();
    
    public Listener OnDisabledListener { get; } = new();

    public override string Prefix => "mpl";

    public override string Author => "SpGerg";

    public string ScriptsPath { get; private set; }
    
    public string DataPath { get; private set; }
    
    public string ConfigsPath { get;  set; }
    
    public override void OnEnabled()
    {
        Instance = this;

        ScriptsPath = Path.Combine(ConfigPath, "scripts");
        DataPath = Path.Combine(ScriptsPath, "data");
        ConfigsPath = Path.Combine(ScriptsPath, "configs");
        
        Directory.CreateDirectory(ScriptsPath);
        Directory.CreateDirectory(DataPath);
        Directory.CreateDirectory(ConfigsPath);

        if (Config.AutoScripts is not null)
        {
            foreach (var script in Config.AutoScripts)
            {
                var path = Path.Combine(ScriptsPath, script);
            
                if (!File.Exists(path))
                {
                    Log.Warn($"Auto script with {script} filepath not found");
                
                    continue;
                }

                var source = File.ReadAllText(path);

                Script.Run(source, Path.GetFileName(path), out _, out _);
            }
        }

        OnEnabledListener.Invoke(null, Location.Default);
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        OnDisabledListener.Invoke(null, Location.Default);
        
        base.OnDisabled();
    }
}