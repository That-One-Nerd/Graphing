using Nerd_STF.Mathematics;
using System.Diagnostics.CodeAnalysis;

namespace Graphing.Graphs;

public class Graph2d(string name) : GraphBase(name)
{
    protected override IReadOnlyList<string> DefaultAxes => ["x", "y"];

    public Float2 Center { get; set; } = (0, 0);
    public Float2? Size { get; set; } = null;

    public Float2? Min
    {
        get
        {
            if (!Size.HasValue) return null;
            else return -Size.Value / 2 + Center;
        }
    }
    public Float2? Max
    {
        get
        {
            if (!Size.HasValue) return null;
            else return Size.Value / 2 + Center;
        }
    }
}
