using System.ComponentModel;
using Exiled.API.Interfaces;

namespace MiniProgrammingLanguage.Exiled;

public class Config : IConfig
{
    [Description("Enabled or not")]
    public bool IsEnabled { get; set; } = true;
    
    [Description("Debug or not")]
    public bool Debug { get; set; }
    
    [Description("Scripts that will start on server start")]
    public string[] AutoScripts { get; set; } = {};
}