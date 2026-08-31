namespace Graphing.Variables;

public class VariableContext
{
    public required Dictionary<string, IGraphVariable> GraphVariables { get; init; }
    public required Dictionary<string, IGraphVariable> LocalVariables { get; init; }
}
