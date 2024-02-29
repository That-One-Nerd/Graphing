using Graphing.Forms;
using Graphing.Parts;

namespace Graphing.Graphables;

public class TangentLine : Graphable
{
    public double Position { get; set; }

    private readonly EquationDelegate parentEqu;

    private readonly double length;

    public TangentLine(double length, double position, Equation parent)
    {
        Name = $"Tangent Line of {parent.Name}";

        parentEqu = parent.GetDelegate();
        Position = position;
        this.length = length;
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        Float2 point = new(Position, parentEqu(Position));
        return [MakeSlopeLine(point, DerivativeAtPoint(Position)),
                new GraphCircle(point, 8)];
    }
    private GraphLine MakeSlopeLine(Float2 position, double slope)
    {
        double dirX = length, dirY = slope * length;
        double magnitude = Math.Sqrt(dirX * dirX + dirY * dirY);

        dirX /= magnitude * 2 / length;
        dirY /= magnitude * 2 / length;

        return new(new(position.x + dirX, position.y + dirY), new(position.x - dirX, position.y - dirY));
    }
    private double DerivativeAtPoint(double x)
    {
        const double step = 1e-3;
        return (parentEqu(x + step) - parentEqu(x)) / step;
    }

    public override void EraseCache() { }
    public override long GetCacheBytes() => 0;
}
