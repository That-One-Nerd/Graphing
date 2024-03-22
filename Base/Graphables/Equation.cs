using Graphing.Abstract;
using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;

namespace Graphing.Graphables;

public class Equation : Graphable, IIntegrable, IDerivable, ITranslatableXY
{
    private static int equationNum;

    public double OffsetX { get; set; }
    public double OffsetY { get; set; }

    protected readonly EquationDelegate equ;
    protected readonly List<Float2> cache;

    public event Action<GraphForm> OnInvalidate;

    public Equation(EquationDelegate equ)
    {
        equationNum++;
        Name = $"Equation {equationNum}";

        this.equ = equ;
        cache = [];

        OffsetX = 0;
        OffsetY = 0;

        OnInvalidate = delegate { };
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        const int step = 10;

        double epsilon = Math.Abs(graph.ScreenSpaceToGraphSpace(new Int2(0, 0)).x
                                - graph.ScreenSpaceToGraphSpace(new Int2(step / 2, 0)).x) / 5;
        epsilon *= graph.DpiFloat / 192;

        List<IGraphPart> lines = [];

        double previousX = graph.MinVisibleGraph.x;
        double previousY = GetFromCache(previousX, epsilon);

        for (int i = 0; i < graph.ClientRectangle.Width + step; i += step)
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
        OnInvalidate.Invoke(graph);
        return lines;
    }

    public Graphable Derive() => new Equation(x =>
    {
        const double step = 1e-3;
        return (equ(x + step) - equ(x)) / step;
    });
    public Graphable Integrate() => new IntegralEquation(this);

    public EquationDelegate GetDelegate() => equ;

    public override void EraseCache() => cache.Clear();
    protected double GetFromCache(double x, double epsilon)
    {
        (double dist, double nearest, int index) = NearestCachedPoint(x - OffsetX);
        if (dist < epsilon) return nearest + OffsetY;
        else
        {
            double result = equ(x - OffsetX);
            cache.Insert(index + 1, new(x - OffsetX, result));
            return result + OffsetY;
        }
    }

    public double GetValueAt(double x) => GetFromCache(x, 0);

    protected (double dist, double y, int index) NearestCachedPoint(double x)
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

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor)
    {
        Int2 screenMousePos = graph.GraphSpaceToScreenSpace(graphMousePos);

        (_, _, int index) = NearestCachedPoint(graphMousePos.x);
        Int2 screenCachePos = graph.GraphSpaceToScreenSpace(cache[index]);

        double allowedDist = factor * graph.DpiFloat * 80 / 192;

        Int2 dist = new(screenCachePos.x - screenMousePos.x,
                        screenCachePos.y - screenMousePos.y);
        double totalDist = Math.Sqrt(dist.x * dist.x + dist.y * dist.y);
        return totalDist <= allowedDist;
    }
    public override Float2 GetSelectedPoint(in GraphForm graph, Float2 graphMousePos) =>
        new(graphMousePos.x, GetFromCache(graphMousePos.x, 1e-3));

    public override void Preload(Float2 xRange, Float2 yRange, double step)
    {
        for (double x = xRange.x; x <= xRange.y; x += step) GetFromCache(x, step);
    }
}

public delegate double EquationDelegate(double x);
