using Nerd_STF.Mathematics;

namespace Graphing.Graphs;

public class Graph2d(string name) : GraphBase(name)
{
    protected override IReadOnlyList<string> DefaultAxes => ["x", "y"];

    public Float2 Center { get; set; } = (0, 0);
    public Float2? Size { get; set; } = null;
}
