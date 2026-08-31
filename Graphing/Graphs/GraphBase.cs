using Graphing.Objects;
using Graphing.Variables;

namespace Graphing.Graphs;

public abstract class GraphBase
{
    public string Name { get; }
    public IReadOnlyList<string> Axes { get; init; }

    public Dictionary<string, IGraphVariable> Variables { get; } = [];

    public GraphBase(string name)
    {
        Name = name;
        Axes = DefaultAxes;
    }

    protected abstract IReadOnlyList<string> DefaultAxes { get; }

    public IGraphObject? TryEvaluate(string name)
    {
        if (!Variables.TryGetValue(name, out IGraphVariable? var)) return null;
        else return var.Evaluate(this);
    }
    public TObject? TryEvaluate<TObject>(string name) where TObject : IGraphObject => (TObject?)TryEvaluate(name);
}
