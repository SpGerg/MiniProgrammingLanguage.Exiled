using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Types.System;

public static class ExceptionType
{
    public static UserTypeInstance Instance { get; } = Create();
    
    public static UserTypeInstance Create()
    {
        var id = new TypeVariableMemberInstanceBuilder()
            .SetParent("__exception")
            .SetName("name")
            .SetModule("std")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetDefault(new StringValue(string.Empty))
            .SetType(ObjectTypeValue.String)
            .Build();
        
        var message = new TypeVariableMemberInstanceBuilder()
            .SetParent("__exception")
            .SetName("message")
            .SetModule("std")
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .SetDefault(new StringValue(string.Empty))
            .SetType(ObjectTypeValue.String)
            .Build();

        return new UserTypeInstanceBuilder()
            .SetName("__exception")
            .SetModule("std")
            .AddMember(id)
            .AddMember(message)
            .Build();
    }
}