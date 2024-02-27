using Graphing.Forms;

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

    public override IEnumerable<Line2d> GetItemsToRender(in GraphForm graph)
    {
        const int step = 10;
        double epsilon = Math.Abs(graph.ScreenSpaceToGraphSpace(new Int2(0, 0)).x
                                - graph.ScreenSpaceToGraphSpace(new Int2(step / 2, 0)).x) / 5;

        List<Line2d> lines = [];

        bool addedToDictionary = false;
        double previousX = graph.MinVisibleGraph.x;
        double previousY = GetFromCache(previousX, epsilon, ref addedToDictionary);

        for (int i = 1; i < graph.ClientRectangle.Width; i += step)
        {
            double currentX = graph.ScreenSpaceToGraphSpace(new Int2(i, 0)).x;
            double currentY = GetFromCache(currentX, epsilon, ref addedToDictionary);
            if (Math.Abs(currentY - previousY) <= 10)
            {
                lines.Add(new Line2d(new Float2(previousX, previousY), new Float2(currentX, currentY)));
            }
            previousX = currentX;
            previousY = currentY;
        }

        if (addedToDictionary) cache.Sort((a, b) => a.y.CompareTo(b.y));

        return lines;
    }

    public EquationDelegate GetDelegate() => equ;

    public void EraseCache() => cache.Clear();
    private double GetFromCache(double x, double epsilon, ref bool addedToDictionary)
    {
        (double dist, double nearest) = NearestCachedPoint(x);
        if (dist < epsilon) return nearest;
        else
        {
            addedToDictionary = true;
            double result = equ(x);
            cache.Add(new(x, result));
            // TODO: Rather than sorting the whole list when we add a single number,
            //       we could just insert it.
            return result;
        }
    }

    private (double dist, double y) NearestCachedPoint(double x)
    {
        // TODO: Replace with a binary search system.
        double closestDist = double.PositiveInfinity;
        double closest = 0;

        foreach (Float2 p in cache)
        {
            double dist = Math.Abs(x - p.x);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = p.y;
            }
        }

        return (closestDist, closest);
    }

    public long GetCacheBytes() => cache.Count * 16;
}

public delegate double EquationDelegate(double x);
