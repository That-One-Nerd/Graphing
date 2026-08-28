namespace Graphing.Graphs;

public abstract class GraphBase
{
    public string Name { get; }
    public IReadOnlyList<string> Axes { get; init; }

    public GraphBase(string name)
    {
        Name = name;
        Axes = DefaultAxes;
    }

    protected abstract IReadOnlyList<string> DefaultAxes { get; }
}
