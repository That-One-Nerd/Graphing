using Graphing.Forms;
using Graphing.Parts;

namespace Graphing.Graphables;

public class Equation : Graphable
{
    private static int equationNum;

    private readonly EquationDelegate equ;

    private readonly List<Float2> cache;

    public Equation(EquationDelegate equ)
    {
        equationNum++;
        Name = $"Equation {equationNum}";

        this.equ = equ;
        cache = [];
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        const int step = 10;
        double epsilon = Math.Abs(graph.ScreenSpaceToGraphSpace(new Int2(0, 0)).x
                                - graph.ScreenSpaceToGraphSpace(new Int2(step / 2, 0)).x) / 5;

        List<IGraphPart> lines = [];

        double previousX = graph.MinVisibleGraph.x;
        double previousY = GetFromCache(previousX, epsilon);

        for (int i = 1; i < graph.ClientRectangle.Width; i += step)
        {
            double currentX = graph.ScreenSpaceToGraphSpace(new Int2(i, 0)).x;
            double currentY = GetFromCache(currentX, epsilon);
            if (Math.Abs(currentY - previousY) <= 10)
            {
                lines.Add(new GraphLine(new Float2(previousX, previousY), new Float2(currentX, currentY)));
            }
            previousX = currentX;
            previousY = currentY;
        }

        // if (addedToDictionary) cache.Sort((a, b) => a.y.CompareTo(b.y)); todo: not required until binary search

        return lines;
    }

    public EquationDelegate GetDelegate() => equ;

    public override void EraseCache() => cache.Clear();
    private double GetFromCache(double x, double epsilon)
    {
        (double dist, double nearest, int index) = NearestCachedPoint(x);
        if (dist < epsilon) return nearest;
        else
        {
            double result = equ(x);
            cache.Add(new(x, result));
            // TODO: Rather than sorting the whole list when we add a single number,
            //       we could just insert it in its proper place.
            return result;
        }
    }

    private (double dist, double y, int index) NearestCachedPoint(double x)
    {
        // TODO: Replace with a binary search system.
        double closestDist = double.PositiveInfinity;
        double closest = 0;
        int closestIndex = -1;

        for (int i = 0; i < cache.Count; i++)
        {
            double dist = Math.Abs(x - cache[i].x);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = cache[i].y;
                closestIndex = i;
            }
        }

        return (closestDist, closest, closestIndex);
    }

    public override long GetCacheBytes() => cache.Count * 16;
}

public delegate double EquationDelegate(double x);
