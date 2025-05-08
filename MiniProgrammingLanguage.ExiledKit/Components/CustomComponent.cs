using System;
using MiniProgrammingLanguage.Core;
using MiniProgrammingLanguage.Core.Interpreter;
using MiniProgrammingLanguage.Core.Interpreter.Repositories.Functions;
using MiniProgrammingLanguage.Core.Interpreter.Values;
using MiniProgrammingLanguage.Core.Parser.Ast;
using UnityEngine;

namespace MiniProgrammingLanguage.ExiledKit.Components;

public class CustomComponent : MonoBehaviour
{
    public string Name { get; private set; }

    public ProgramContext ProgramContext { get; private set; }
    
    public FunctionBodyExpression Root { get; private set; }
    
    public Location Location { get; private set; }
    
    public FunctionValue AwakeFunction { get; set; }
    
    public FunctionValue UpdateFunction { get; set; }

    private FunctionExecuteContext _functionExecuteContext;

    public void Initialize(string componentName, ProgramContext programContext, FunctionBodyExpression root, Location location)
    {
        Name ??= componentName;
        ProgramContext ??= programContext;
        Root ??= root;
        Location = location;

        _functionExecuteContext ??= new FunctionExecuteContext
        {
            ProgramContext = ProgramContext,
            Arguments = Array.Empty<AbstractEvaluableExpression>(),
            Root = Root,
            Location = location
        };
    }

    public void Awake()
    {
        AwakeFunction?.Value.Evaluate(_functionExecuteContext);
    }
    
    public void Update()
    {
        UpdateFunction?.Value.Evaluate(_functionExecuteContext);
    }
}