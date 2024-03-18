using Graphing.Forms;
using Graphing.Parts;

namespace Graphing.Graphables;

public class ColumnTable : Graphable
{
    private static int tableNum;

    protected readonly Dictionary<double, double> tableXY;
    protected readonly double width;

    public ColumnTable(double width, Dictionary<double, double> tableXY)
    {
        tableNum++;
        Name = $"Column Table {tableNum}";

        this.tableXY = tableXY;
        this.width = width;
    }
    public ColumnTable(double step, Equation equation, double min, double max)
    {
        Name = $"Column Table for {equation.Name}";

        tableXY = [];
        EquationDelegate equ = equation.GetDelegate();
        width = 0.75 * step;

        double minRounded = Math.Round(min / step) * step,
               maxRounded = Math.Round(max / step) * step;
        for (double x = minRounded; x <= maxRounded; x += step)
            tableXY.Add(x, equ(x));
    }

    public override void EraseCache() { }
    public override long GetCacheBytes() => 16 * tableXY.Count;

    public override Graphable DeepCopy() => new ColumnTable(width / 0.75, tableXY.ToArray().ToDictionary());

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        List<IGraphPart> items = [];
        foreach (KeyValuePair<double, double> col in tableXY)
        {
            items.Add(GraphRectangle.FromSize(new Float2(col.Key, col.Value / 2),
                                              new Float2(width, col.Value)));
        }

        return items;
    }

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor) => false;
    public override Float2 GetSelectedPoint(in GraphForm graph, Float2 graphMousePos) => default;

    // Nothing to preload, everything is already cached.
    public override void Preload(Float2 xRange, Float2 yRange, double step) { }
}
