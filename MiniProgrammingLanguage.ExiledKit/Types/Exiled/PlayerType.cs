using Exiled.API.Features;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Types;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Interpreter.Values.Enums;
using MiniProgrammingLanguage.Core.Interpreter.Values.EnumsValues;
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
            .AddMember(new TypeLanguageVariableMemberInstanceBuilder()
                .SetParent("ex_player")
                .SetName("nickname")
                .SetAccess(AccessType.ReadOnly | AccessType.Static)
                .SetGetBind(GetNickname)
                .Build())
            .AddMember(new TypeLanguageVariableMemberInstanceBuilder()
                .SetParent("ex_player")
                .SetName("rank")
                .SetAccess(AccessType.ReadOnly | AccessType.Static)
                .SetGetBind(GetRank)
                .SetSetBind(SetRank)
                .SetType(ObjectTypeValue.String)
                .Build())
            .AddMember(new TypeLanguageFunctionMemberInstanceBuilder()
                .SetParent("ex_player")
                .SetName("get_user_id")
                .SetAccess(AccessType.ReadOnly | AccessType.Static)
                .SetBind(GetUserId)
                .Build())
            .AddMember(new TypeLanguageFunctionMemberInstanceBuilder()
                .SetParent("ex_player")
                .SetName("get_gameobject")
                .SetAccess(AccessType.ReadOnly | AccessType.Static)
                .SetBind(GetGameObject)
                .SetReturn(new ObjectTypeValue("un_game_object", ValueType.CSharpObject))
                .Build())
            .AddMember(new TypeLanguageVariableMemberInstanceBuilder()
                .SetParent("ex_player")
                .SetName("role")
                .SetAccess(AccessType.Static)
                .SetGetBind(GetRole)
                .SetSetBind(SetRole)
                .SetType(new ObjectTypeValue(RoleTypeEnum.Instance.Name, ValueType.EnumMember))
                .Build())
            .AddMember(new TypeLanguageFunctionMemberInstanceBuilder()
                .SetParent("ex_player")
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
    
    public static AbstractValue GetRank(TypeMemberGetterContext context)
    {
        var player = (Player) context.Type.ObjectTarget;
        
        return new StringValue(player.RankName);
    }
    
    public static void SetRank(TypeMemberSetterContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        player.RankName = context.Value.AsString(context.ProgramContext, context.Location);
    }
    
    public static AbstractValue GetUserId(TypeFunctionExecuteContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        return new StringValue(player.UserId);
    }
    
    public static AbstractValue GetNickname(TypeMemberGetterContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        return new StringValue(player.Nickname);
    }
    
    public static AbstractValue GetGameObject(TypeFunctionExecuteContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        return new CSharpObjectValue("un_game_object", player.GameObject);
    }
    
    public static AbstractValue GetRole(TypeMemberGetterContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        var value = (int) player.Role.Type;
        var name = RoleTypeEnum.Instance.GetByValue(value);

        return new EnumMemberValue(RoleTypeEnum.Instance, name, value);
    }
    
    public static void SetRole(TypeMemberSetterContext context)
    {
        var player = (Player) context.Type.ObjectTarget;

        var role = (RoleTypeId) context.Value.AsRoundNumber(context.ProgramContext, context.Location);

        player.Role.Set(role);
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