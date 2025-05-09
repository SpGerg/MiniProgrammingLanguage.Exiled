using System;
using System.IO;
using System.Text;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types.Identifications;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Functions.Logger;
using MiniProgrammingLanguage.ExiledKit.Types.System.Serializable;
using Newtonsoft.Json;

namespace MiniProgrammingLanguage.ExiledKit.Types.System;

public static class SaverType
{
    public static UserTypeInstance Instance { get; } = Create();

    public static UserTypeInstance Create()
    {
        return new UserTypeInstanceBuilder()
            .SetName("saver")
            .SetAccess(AccessType.Static)
            .AddMember(new TypeVariableMemberInstanceBuilder()
                .SetParent("saver")
                .SetName("filepath")
                .SetAccess(AccessType.Static | AccessType.ReadOnly)
                .SetType(ObjectTypeValue.String)
                .Build())
            .AddMember(new TypeLanguageFunctionMemberInstanceBuilder()
                .SetParent("saver")
                .SetName("get")
                .SetBind(Get)
                .SetArguments(new FunctionArgument("name", ObjectTypeValue.String))
                .SetAccess(AccessType.Static | AccessType.ReadOnly)
                .Build())
            .AddMember(new TypeLanguageFunctionMemberInstanceBuilder()
                .SetParent("saver")
                .SetName("set")
                .SetBind(Set)
                .SetArguments(new FunctionArgument("name", ObjectTypeValue.String),
                    new FunctionArgument("value", ObjectTypeValue.Any))
                .SetAccess(AccessType.Static | AccessType.ReadOnly)
                .Build())
            .Build();
    }

    public static AbstractValue Get(TypeFunctionExecuteContext context)
    {
        var nameArgument = context.Arguments[0];

        var name = nameArgument.Evaluate(context.ProgramContext).AsString(context.ProgramContext, context.Location);
        
        var filePathMember = (TypeMemberValue) context.Type.Get(new KeyTypeMemberIdentification { Identifier = "filepath" });
        var filePath = filePathMember.Value.AsString(context.ProgramContext, context.Location);

        var dataFilePath = Path.Combine(filePath, $"{context.ProgramContext.ExecutorName}.data");

        if (!File.Exists(dataFilePath))
        {
            return NoneValue.Instance;
        }
        
        using var fileLock =
            new FileStream(dataFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        
        using var fileReader = new StreamReader(fileLock, Encoding.UTF8);
        var data = fileReader.ReadToEnd();
        
        var json = string.IsNullOrEmpty(data)
            ? null
            : JsonConvert.DeserializeObject<SaveDataSerializable>(data);

        if (json is null)
        {
            return NoneValue.Instance;
        }

        if (!json.Values.TryGetValue(name, out var value))
        {
            return NoneValue.Instance;
        }

        return value.Deserialize(context.ProgramContext);
    }

    public static AbstractValue Set(TypeFunctionExecuteContext context)
    {
        var nameArgument = context.Arguments[0];
        var valueArgument = context.Arguments[1];

        var name = nameArgument.Evaluate(context.ProgramContext).AsString(context.ProgramContext, context.Location);
        var value = valueArgument.Evaluate(context.ProgramContext);
        
        var filePathMember = (TypeMemberValue) context.Type.Get(new KeyTypeMemberIdentification { Identifier = "filepath" });
        var filePath = filePathMember.Value.AsString(context.ProgramContext, context.Location);

        var dataFilePath = Path.Combine(filePath, $"{context.ProgramContext.ExecutorName}.data");

        using var fileLock =
            new FileStream(dataFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        using var fileReader = new StreamReader(fileLock, Encoding.UTF8);
        var data = fileReader.ReadToEnd();
            
        var json = string.IsNullOrEmpty(data)
            ? new SaveDataSerializable()
            : JsonConvert.DeserializeObject<SaveDataSerializable>(data);

        var serialized = SerializableValueFactory.Serialize(value, context.Location);

        if (json.Values.ContainsKey(name))
        {
            json.Values[name] = serialized;
        }
        else
        {
            json.Values.Add(name, serialized);
        }
            
        fileLock.SetLength(0);
            
        using var fileWriter = new StreamWriter(fileLock, Encoding.UTF8);
        fileWriter.Write(JsonConvert.SerializeObject(json));

        return VoidValue.Instance;
    }
}