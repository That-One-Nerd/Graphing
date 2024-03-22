using Graphing.Abstract;
using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;

namespace Graphing.Graphables;

public class TangentLine : Graphable, IEquationConvertible, ITranslatableX
{
    public double Position
    {
        get => _position;
        set
        {
            currentSlope = DerivativeAtPoint(value);
            _position = value;
        }
    }
    private double _position; // Private because it has exactly the same functionality as `Position`.

    public double OffsetX
    {
        get => Position;
        set => Position = value;
    }

    protected readonly Equation parent;

    protected readonly double length;

    // X is slope, Y is height.
    protected Float2 currentSlope;

    // No binary search for this, I want it to be exact.
    // Value: X is slope, Y is height.
    protected Dictionary<double, Float2> slopeCache;

    public TangentLine(double length, double position, Equation parent)
    {
        Name = $"Tangent Line of {parent.Name}";

        slopeCache = [];
        this.length = length;
        this.parent = parent;
        Position = position;

        parent.OnInvalidate += (graph) =>
        {
            // I don't love this but it works.
            EraseCache();
            Position = _position; // Done for side effects.
        };
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        Float2 point = new(Position, currentSlope.y);
        return [MakeSlopeLine(), new GraphUiCircle(point, 8)];
    }
    protected GraphLine MakeSlopeLine()
    {
        double dirX = length, dirY = currentSlope.x * length;
        double magnitude = Math.Sqrt(dirX * dirX + dirY * dirY);

        dirX /= magnitude * 2 / length;
        dirY /= magnitude * 2 / length;

        return new(new(Position + dirX, currentSlope.y + dirY), new(Position - dirX, currentSlope.y - dirY));
    }
    protected Float2 DerivativeAtPoint(double x)
    {
        // If value is already computed, return it.
        if (slopeCache.TryGetValue(x, out Float2 val)) return val;

        const double step = 1e-3;

        double initial = parent.GetValueAt(x);
        Float2 result = new((parent.GetValueAt(x + step) - initial) / step, initial);
        slopeCache.Add(x, result);
        return result;
    }

    public override Graphable DeepCopy() => new TangentLine(length, Position, parent);

    public override void EraseCache() => slopeCache.Clear();
    public override long GetCacheBytes() => slopeCache.Count * 24;

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor)
    {
        GraphLine line = MakeSlopeLine();

        if (graphMousePos.x < Math.Min(line.a.x - 0.25, line.b.x - 0.25) ||
            graphMousePos.x > Math.Max(line.a.x + 0.25, line.b.x + 0.25)) return false;

        double allowedDist = factor * graph.DpiFloat * 80 / 192;

        double lineX = graphMousePos.x,
               lineY = currentSlope.x * (lineX - Position) + currentSlope.y;

        Int2 pointScreen = graph.GraphSpaceToScreenSpace(new Float2(lineX, lineY));
        Int2 mouseScreen = graph.GraphSpaceToScreenSpace(graphMousePos);
        Int2 dist = new(pointScreen.x - mouseScreen.x,
                        pointScreen.y - mouseScreen.y);
        double totalDist = Math.Sqrt(dist.x * dist.x + dist.y * dist.y);
        return totalDist <= allowedDist;
    }
    public override Float2 GetSelectedPoint(in GraphForm graph, Float2 graphMousePos)
    {
        GraphLine line = MakeSlopeLine();

        double lineX = Math.Clamp(graphMousePos.x,
                                  Math.Min(line.a.x, line.b.x),
                                  Math.Max(line.a.x, line.b.x)),
               lineY = currentSlope.x * (lineX - Position) + currentSlope.y;
        return new Float2(lineX, lineY);
    }

    public override void Preload(Float2 xRange, Float2 yRange, double step)
    {
        // Despite the tangent line barely using any data, when preloaded it
        // will always take as much memory as an equation. Seems like a bit much,
        // but may be used when the tangent line is moved. Not sure there's much
        // that can be changed.
        for (double x = xRange.x; x <= xRange.y; x += step) DerivativeAtPoint(x);
    }

    public Equation ToEquation()
    {
        double slope = currentSlope.x, x1 = Position, y1 = currentSlope.y;
        return new(x => slope * (x - x1) + y1)
        {
            Name = Name,
            Color = Color
        };
    }
}
