using System.IO;
using Exiled.API.Features;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.ExiledKit;

namespace MiniProgrammingLanguage.Exiled;

public class Plugin : Plugin<Config>
{
    public static Plugin Instance { get; private set; }

    public Listener OnEnabledListener { get; } = new();
    
    public Listener OnDisabledListener { get; } = new();

    public override string Prefix => "mpl";

    public override string Author => "SpGerg";

    public string ScriptsPath { get; private set; }
    
    public override void OnEnabled()
    {
        Instance = this;

        ScriptsPath = Path.Combine(ConfigPath, "scripts");
        
        OnEnabledListener.Invoke(null, Location.Default);

        Directory.CreateDirectory(ScriptsPath);

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        OnDisabledListener.Invoke(null, Location.Default);
        
        base.OnDisabled();
    }
}