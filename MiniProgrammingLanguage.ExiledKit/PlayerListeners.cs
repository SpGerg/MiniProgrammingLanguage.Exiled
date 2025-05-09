using MiniProgrammingLanguage.Core.Interpreter.Values.Type;
using MiniProgrammingLanguage.ExiledKit.Types.System;

namespace MiniProgrammingLanguage.ExiledKit;

public static class PlayerListeners
{
    public static Listener VerifiedListener { get; } = new();
    
    public static TypeValue Verified { get; } = ListenerType.CreateValue(VerifiedListener);
    
    public static Listener LeftListener { get; } = new();
    
    public static TypeValue Left { get; } = ListenerType.CreateValue(LeftListener);
    
    public static Listener DiedListener { get; } = new();
    
    public static TypeValue Died { get; } = ListenerType.CreateValue(DiedListener);
}