using MiniProgrammingLanguage.Core.Interpreter.Repositories.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Variables;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;

namespace MiniProgrammingLanguage.ExiledKit.Enums.Exiled;

public static class RoleTypeEnum
{
    public static UserEnumInstance Instance { get; } = Create();
    
    public static UserVariableInstance VariableInstance { get; } = new()
    {
        Name = Instance.Name,
        Module = ExiledKitModule.Name,
        Type = new ObjectTypeValue(Instance.Name, ValueType.Enum),
        Access = AccessType.Static | AccessType.ReadOnly,
        Value = Instance.Create(),
        Root = null
    };
    
    public static UserEnumInstance Create()
    {
        return new UserEnumInstanceBuilder()
            .SetName("role_type")
            .SetModule(ExiledKitModule.Name)
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .AddMember("none", -1)
            .AddMember("scp_173", 0)
            .AddMember("class_d", 1)
            .AddMember("spectator", 2)
            .AddMember("scp_106", 3)
            .AddMember("ntf_specialist", 4)
            .AddMember("scp_049", 5)
            .AddMember("scientist", 6)
            .AddMember("scp_079", 7)
            .AddMember("chaos_conscript", 8)
            .AddMember("scp_096", 9)
            .AddMember("scp_0492", 10)
            .AddMember("ntf_sergeant", 11)
            .AddMember("ntf_captain", 12)
            .AddMember("ntf_private", 13)
            .AddMember("tutorial", 14)
            .AddMember("facility_guard", 15)
            .AddMember("scp_939", 16)
            .AddMember("custom_role", 17)
            .AddMember("chaos_rifleman", 18)
            .AddMember("chaos_marauder", 19)
            .AddMember("chaos_repressor", 20)
            .AddMember("overwatch", 21)
            .AddMember("filmmaker", 22)
            .AddMember("scp3114", 23)
            .AddMember("flamingo", 24)
            .AddMember("alpha_flamingo", 25)
            .AddMember("zombie_flamingo", 26)
            .Build();
    }
}