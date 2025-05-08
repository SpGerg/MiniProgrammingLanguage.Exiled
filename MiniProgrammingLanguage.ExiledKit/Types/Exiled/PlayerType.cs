using Exiled.API.Features;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.Core.Parser;
using MiniProgrammingLanguage.Core.Parser.Ast.Enums;
using MiniProgrammingLanguage.ExiledKit.Enums.Exiled;
using PlayerRoles;

namespace MiniProgrammingLanguage.ExiledKit.Types.Exiled;

public static class PlayerType
{
    public static UserTypeInstance Instance { get; } = Create();
    
    public static UserTypeInstance Create()
    {
        return new UserTypeInstanceBuilder()
            .SetName("ex_player")
            .SetModule(ExiledKitModule.Name)
            .SetAccess(AccessType.Static | AccessType.ReadOnly)
            .AddMember(new TypeLanguageFunctionMemberInstanceBuilder()
                .SetName("set_role")
                .SetAccess(AccessType.Static)
                .SetArguments(new FunctionArgument("role", new ObjectTypeValue(RoleTypeEnum.Instance.Name, ValueType.EnumMember)))
                .SetBind(SetRole)
                .Build())
            .AddMember(new TypeLanguageFunctionMemberInstanceBuilder()
                .SetName("broadcast")
                .SetAccess(AccessType.Static)
                .SetArguments(new FunctionArgument("duration", ObjectTypeValue.RoundNumber),
                    new FunctionArgument("content", ObjectTypeValue.String))
                .SetBind(Broadcast)
                .Build())
            .Build();
    }

    public static TypeValue CreateValue(Player player)
    {
        var type = Instance.Create();
        type.ObjectTarget = player;

        return type;
    }
    
    public static AbstractValue SetRole(TypeFunctionExecuteContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        var roleArgument = context.Arguments[0];

        var role = (RoleTypeId) roleArgument.Evaluate(context.ProgramContext).AsRoundNumber(context.ProgramContext, context.Location);

        player.Role.Set(role);
        
        return VoidValue.Instance;
    }

    public static AbstractValue Broadcast(TypeFunctionExecuteContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        var durationArgument = context.Arguments[0];
        var contentArgument = context.Arguments[1];

        var duration = (ushort) durationArgument.Evaluate(context.ProgramContext).AsRoundNumber(context.ProgramContext, context.Location);
        var content = contentArgument.Evaluate(context.ProgramContext).AsString(context.ProgramContext, context.Location);
        
        player.Broadcast(duration, content);
        
        return VoidValue.Instance;
    }
}