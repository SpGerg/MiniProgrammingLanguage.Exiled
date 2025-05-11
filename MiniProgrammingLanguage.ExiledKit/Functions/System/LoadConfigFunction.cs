using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Exiled.API.Interfaces;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types.Identifications;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Interfaces;
using MiniProgrammingLanguage.ExiledKit.Types.Exiled;
using MiniProgrammingLanguage.SharpKit.Factory;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MiniProgrammingLanguage.ExiledKit.Functions.System;

public static class LoadConfigFunction
{
    public static LanguageFunctionInstance Instance { get; } = Create();

    private static readonly ISerializer Serializer = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();
    
    public static LanguageFunctionInstance Create()
    {
        return new LanguageFunctionInstanceBuilder()
            .SetName("load_config")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetArguments(
                new FunctionArgument("plugin", new ObjectTypeValue(PluginType.Instance.Name, ValueType.Type)), 
                new FunctionArgument("config", ObjectTypeValue.Object))
            .SetBind(LoadConfig)
            .Build();
    }

    public static AbstractValue LoadConfig(LanguageFunctionExecuteContext context)
    {
        var pluginType = (TypeValue) context.Arguments[0];
        var plugin = (IExiledKitPlugin<IConfig>) pluginType.ObjectTarget;
        var path = Path.Combine(plugin.ConfigsPath, $"{context.ProgramContext.ExecutorName}.config");

        var configType = (TypeValue) context.Arguments[1];
        var dictionary = new Dictionary<string, object>();

        foreach (var member in configType.Members)
        {
            if (member.Key is not KeyTypeMemberIdentification identification || member.Value is not TypeMemberValue typeMemberValue)
            {
                continue;
            }

            var value = TypesFactory.Create(typeMemberValue.Value);
            
            dictionary.Add(identification.Identifier, value);
        }

        var serialized = Serializer.Serialize(dictionary);
        
        File.WriteAllText(path, serialized);
        
        return configType;
    }
}