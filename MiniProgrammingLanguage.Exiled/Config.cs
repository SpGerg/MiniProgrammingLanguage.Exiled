using Exiled.API.Interfaces;

namespace MiniProgrammingLanguage.Exiled;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    
    public bool Debug { get; set; }
}