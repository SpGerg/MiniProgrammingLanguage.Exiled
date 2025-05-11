using Exiled.API.Interfaces;

namespace MiniProgrammingLanguage.ExiledKit.Interfaces;

public interface IExiledKitPlugin<out T> : IPlugin<T> where T : IConfig
{
    string ScriptsPath { get; }
    
    string DataPath { get; }
    
    string ConfigsPath { get; }
}