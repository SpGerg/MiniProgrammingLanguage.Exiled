using System.IO;
using Exiled.API.Interfaces;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Interfaces;
using MiniProgrammingLanguage.ExiledKit.Types.Exiled;

namespace MiniProgrammingLanguage.ExiledKit.Functions.System;

public static class GetDataDirectoryFunction
{
    public static LanguageFunctionInstance Instance { get; } = Create();
    
    public static LanguageFunctionInstance Create()
    {
        return new LanguageFunctionInstanceBuilder()
            .SetName("get_data_directory")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetArguments(new FunctionArgument("plugin", new ObjectTypeValue(PluginType.Instance.Name, ValueType.Type)))
            .SetBind(GetDataDirectory)
            .Build();
    }

    public static AbstractValue GetDataDirectory(LanguageFunctionExecuteContext context)
    {
        var pluginType = (TypeValue) context.Arguments[0];
        var plugin = (IExiledKitPlugin<IConfig>) pluginType.ObjectTarget;

        return new StringValue(plugin.DataPath);
    }
}