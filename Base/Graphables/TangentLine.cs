using Graphing.Forms;
using Graphing.Parts;

namespace Graphing.Graphables;

public class TangentLine : Graphable
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
    private double _position;

    protected readonly Equation parent;
    protected readonly EquationDelegate parentEqu;

    protected readonly double length;

    protected double currentSlope;

    // No binary search for this, I want it to be exact.
    protected Dictionary<double, double> slopeCache;

    public TangentLine(double length, double position, Equation parent)
    {
        Name = $"Tangent Line of {parent.Name}";

        slopeCache = [];
        parentEqu = parent.GetDelegate();
        Position = position;
        this.length = length;
        this.parent = parent;
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        Float2 point = new(Position, parentEqu(Position));
        return [MakeSlopeLine(point, currentSlope),
                new GraphUiCircle(point, 8)];
    }
    protected GraphLine MakeSlopeLine(Float2 position, double slope)
    {
        double dirX = length, dirY = slope * length;
        double magnitude = Math.Sqrt(dirX * dirX + dirY * dirY);

        dirX /= magnitude * 2 / length;
        dirY /= magnitude * 2 / length;

        return new(new(position.x + dirX, position.y + dirY), new(position.x - dirX, position.y - dirY));
    }
    protected double DerivativeAtPoint(double x)
    {
        // If value is already computed, return it.
        if (slopeCache.TryGetValue(x, out double y)) return y;

        const double step = 1e-3;
        double result = (parentEqu(x + step) - parentEqu(x)) / step;
        slopeCache.Add(x, result);
        return result;
    }

    public override Graphable DeepCopy() => new TangentLine(length, Position, parent);

    public override void EraseCache() => slopeCache.Clear();
    public override long GetCacheBytes() => slopeCache.Count * 16;

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor) => false;
    public override Float2 GetSelectedPoint(in GraphForm graph, Float2 graphMousePos) => default;

    public override void Preload(Float2 xRange, Float2 yRange, double step)
    {
        // Despite the tangent line barely using any data, when preloaded it
        // will always take as much memory as an equation. Seems like a bit much,
        // but may be used when the tangent line is moved. Not sure there's much
        // that can be changed.
        for (double x = xRange.x; x <= xRange.y; x += step) DerivativeAtPoint(x);
    }
}
