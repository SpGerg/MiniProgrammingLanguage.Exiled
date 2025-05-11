using System.IO;
using Exiled.API.Features;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.ExiledKit;
using MiniProgrammingLanguage.ExiledKit.Interfaces;

namespace MiniProgrammingLanguage.Exiled;

public class Plugin : Plugin<Config>, IExiledKitPlugin<Config>
{
    public static Plugin Instance { get; private set; }

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

        OnEnabledListener.Invoke(null, Location.Default);
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        OnDisabledListener.Invoke(null, Location.Default);
        
        base.OnDisabled();
    }
}