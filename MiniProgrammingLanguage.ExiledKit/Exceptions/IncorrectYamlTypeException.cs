using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Exceptions;

namespace MiniProgrammingLanguage.ExiledKit.Exceptions;

public class IncorrectYamlTypeException : AbstractLanguageException
{
    public IncorrectYamlTypeException(string member, string excepted, string received, Location location) : base($"Except '{excepted}' in '{member}', but got '{received}'", location)
    {
    }
}