using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Exiled.API.Interfaces;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types.Identifications;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Exceptions;
using MiniProgrammingLanguage.ExiledKit.Interfaces;
using MiniProgrammingLanguage.ExiledKit.Types.Exiled;
using MiniProgrammingLanguage.SharpKit.Factory;
using PluginAPI.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MiniProgrammingLanguage.ExiledKit.Functions.System;

public static class GetConfigFunction
{
    public static LanguageFunctionInstance Instance { get; } = Create();

    private static readonly IDeserializer Deserializer = new DeserializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();
    
    public static LanguageFunctionInstance Create()
    {
        return new LanguageFunctionInstanceBuilder()
            .SetName("get_config")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetBind(GetConfig)
            .SetArguments(
                new FunctionArgument("plugin", new ObjectTypeValue(PluginType.Instance.Name, ValueType.Type)), 
                new FunctionArgument("config", ObjectTypeValue.Object))
            .SetReturn(ObjectTypeValue.Object)
            .Build();
    }

    public static AbstractValue GetConfig(LanguageFunctionExecuteContext context)
    {
        var pluginType = (TypeValue) context.Arguments[0];
        var plugin = (IExiledKitPlugin<IConfig>) pluginType.ObjectTarget;
        var path = Path.Combine(plugin.ConfigsPath, $"{context.ProgramContext.ExecutorName}.config");

        if (!File.Exists(path))
        {
            return NoneValue.Instance;
        }
        
        var configType = (TypeValue) context.Arguments[1];

        var config = File.ReadAllText(path);
        var deserialized = Deserializer.Deserialize<Dictionary<string, object>>(config);

        foreach (var member in configType.Members)
        {
            if (member.Key is not KeyTypeMemberIdentification identification || member.Value is not TypeMemberValue typeMemberValue)
            {
                continue;
            }

            if (!deserialized.TryGetValue(identification.Identifier, out var configValue))
            {
                continue;
            }

            var configValueString = configValue.ToString();
            AbstractValue value;
            
            if (float.TryParse(configValueString.Replace(".", ","), out var number))
            {
                if (configValueString.Contains("."))
                {
                    value = new NumberValue(number);
                }
                else
                {
                    value = new RoundNumberValue((int) number);
                }
            }
            else
            {
                value = TypesFactory.Create(configValue, context.ProgramContext, out var implementModule);
                context.ProgramContext.Import(implementModule);
            }

            if (!typeMemberValue.Type.Is(value))
            {
                throw new IncorrectYamlTypeException(identification.Identifier, typeMemberValue.Type.ToString(), value.ToString(), context.Location);
            }

            typeMemberValue.Value = value;
        }
        
        return configType;
    }
}