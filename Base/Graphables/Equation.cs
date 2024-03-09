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
            cache.Insert(index + 1, new(x, result));
            return result;
        }
    }

    // Pretty sure this works. Certainly works pretty well with "hard-to-compute"
    // equations.
    private (double dist, double y, int index) NearestCachedPoint(double x)
    {
        if (cache.Count == 0) return (double.PositiveInfinity, double.NaN, -1);
        else if (cache.Count == 1)
        {
            Float2 single = cache[0];
            return (Math.Abs(single.x - x), single.y, 0);
        }
        else
        {
            int boundA = 0, boundB = cache.Count;
            do
            {
                int boundC = (boundA + boundB) / 2;
                Float2 pointC = cache[boundC];

                if (pointC.x == x) return (0, pointC.y, boundC);
                else if (pointC.x > x)
                {
                    boundA = boundC;
                }
                else // pointC.x < x
                {
                    boundB = boundC;
                }

            } while (boundB - boundA > 1);

            return (Math.Abs(cache[boundA].x - x), cache[boundA].y, boundA);
        }
    }

    public override Graphable DeepCopy() => new Equation(equ);

    public override long GetCacheBytes() => cache.Count * 16;
}

public delegate double EquationDelegate(double x);
