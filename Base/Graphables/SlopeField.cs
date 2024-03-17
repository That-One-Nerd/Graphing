using Graphing.Forms;
using Graphing.Parts;

namespace Graphing.Graphables;

public class SlopeField : Graphable
{
    private static int slopeFieldNum;

    protected readonly SlopeFieldsDelegate equ;
    protected readonly int detail;

    protected readonly List<(Float2, GraphLine)> cache;

    public SlopeField(int detail, SlopeFieldsDelegate equ)
    {
        slopeFieldNum++;
        Name = $"Slope Field {slopeFieldNum}";

        this.equ = equ;
        this.detail = detail;
        cache = [];
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        double epsilon = 1 / (detail * 2.0);
        List<IGraphPart> lines = [];

        for (double x = Math.Ceiling(graph.MinVisibleGraph.x - 1); x < graph.MaxVisibleGraph.x + 1; x += 1.0 / detail)
        {
            for (double y = Math.Ceiling(graph.MinVisibleGraph.y - 1); y < graph.MaxVisibleGraph.y + 1; y += 1.0 / detail)
            {
                lines.Add(GetFromCache(epsilon, x, y));
            }
        }

        return lines;
    }

    protected GraphLine MakeSlopeLine(Float2 position, double slope)
    {
        double size = detail;

        double dirX = size, dirY = slope * size;
        double magnitude = Math.Sqrt(dirX * dirX + dirY * dirY);

        dirX /= magnitude * size * 2;
        dirY /= magnitude * size * 2;

        return new(new(position.x + dirX, position.y + dirY), new(position.x - dirX, position.y - dirY));
    }
    protected GraphLine GetFromCache(double epsilon, double x, double y)
    {
        // Probably no binary search here, though maybe it could be done
        // in terms of just one axis.

        foreach ((Float2 p, GraphLine l) in cache)
        {
            double diffX = Math.Abs(p.x - x),
                   diffY = Math.Abs(p.y - y);

            if (diffX < epsilon && diffY < epsilon) return l;
        }

        // Create a new value.
        double slope = equ(x, y);
        GraphLine result = MakeSlopeLine(new Float2(x, y), slope);
        cache.Add((new Float2(x, y), result));
        return result;
    }

    public override Graphable DeepCopy() => new SlopeField(detail, equ);

    public override void EraseCache() => cache.Clear();
    public override long GetCacheBytes() => cache.Count * 48;

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor) => false;
    public override Float2 GetSelectedPoint(in GraphForm graph, Float2 graphMousePos) => default;

    public override void Preload(Float2 xRange, Float2 yRange) { }
}

public delegate double SlopeFieldsDelegate(double x, double y);
