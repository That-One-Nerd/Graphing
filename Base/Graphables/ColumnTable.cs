using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        Color = equation.Color;
        Name = $"Column Table for {equation.Name}";

        tableXY = [];
        EquationDelegate equ = equation.GetDelegate();
        width = 0.75 * step;

        double minRounded = Math.Round(min / step) * step,
               maxRounded = Math.Round(max / step) * step;
        for (double x = minRounded; x <= maxRounded; x += step)
            tableXY.Add(x, equ(x));
    }

    public override long GetCacheBytes() => 16 * tableXY.Count;

    public override Graphable ShallowCopy() => new ColumnTable(width / 0.75, tableXY);

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        List<IGraphPart> items = [];
        foreach (KeyValuePair<double, double> col in tableXY)
        {
            items.Add(GraphRectangle.FromSize(new Float2(col.Key, col.Value / 2),
                                              new Float2(width, col.Value), 0.625));
        }

        return items;
    }

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor)
    {
        // Get closest value to mouse pos.
        double closestDist = double.PositiveInfinity, closestX = 0, closestY = 0;
        foreach (KeyValuePair<double, double> points in tableXY)
        {
            double dist = Math.Abs(points.Key - graphMousePos.x);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestX = points.Key;
                closestY = points.Value;
            }
        }

        Int2 screenMousePos = graph.GraphSpaceToScreenSpace(graphMousePos);
        Int2 minBox = graph.GraphSpaceToScreenSpace(new(closestX - width / 2, 0)),
             maxBox = graph.GraphSpaceToScreenSpace(new(closestX + width / 2, closestY));

        int distX, distY;
        if (screenMousePos.x < minBox.x) distX = minBox.x - screenMousePos.x; // On left side.
        else if (screenMousePos.x > maxBox.x) distX = screenMousePos.x - maxBox.x; // On right side.
        else distX = 0; // Inside.

        if (closestY > 0)
        {
            if (screenMousePos.y > minBox.y) distY = screenMousePos.y - minBox.y; // Underneath.
            else if (screenMousePos.y < maxBox.y) distY = maxBox.y - screenMousePos.y; // Above.
            else distY = 0; // Inside.
        }
        else
        {
            if (screenMousePos.y < minBox.y) distY = minBox.y - screenMousePos.y; // Underneath.
            else if (screenMousePos.y > maxBox.y) distY = screenMousePos.y - maxBox.y; // Above.
            else distY = 0; // Inside.
        }

        int totalDist = (int)Math.Sqrt(distX * distX + distY * distY);
        return totalDist < 50 * factor * graph.DpiFloat / 192;
    }
    public override IEnumerable<IGraphPart> GetSelectionItemsToRender(in GraphForm graph, Float2 graphMousePos)
    {
        // Get closest value to mouse pos.
        double closestDist = double.PositiveInfinity, closestX = 0, closestY = 0;
        foreach (KeyValuePair<double, double> points in tableXY)
        {
            double dist = Math.Abs(points.Key - graphMousePos.x);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestX = points.Key;
                closestY = points.Value;
            }
        }

        Float2 textPoint = new(closestX, closestY);
        Int2 offset;
        ContentAlignment alignment;
        if (textPoint.y >= 0)
        {
            offset = new(0, -5);
            alignment = ContentAlignment.BottomCenter;
        }
        else
        {
            offset = new(0, 5);
            alignment = ContentAlignment.TopCenter;
        }

        return
        [
            new GraphUiText($"{closestY:0.00}", textPoint, alignment, offsetPix: offset)
        ];
    }

    // Nothing to preload, everything is already cached.
    public override void Preload(Float2 xRange, Float2 yRange, double step) { }
}
