using Graphing.Abstract;
using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;

namespace Graphing.Graphables;

public class ParametricEquation : Graphable, IDerivable, ITranslatableXY
{
    private static int equationNum;

    public double OffsetX { get; set; }
    public double OffsetY { get; set; }

    public double InitialT { get; set; }
    public double FinalT { get; set; }

    protected readonly ParametricDelegate equX, equY;
    protected readonly List<(double t, Float2 point)> cache;

    public ParametricEquation(double initialT, double finalT,
                              ParametricDelegate equX, ParametricDelegate equY)
    {
        equationNum++;
        Name = $"Parametric Equation {equationNum}";

        InitialT = initialT;
        FinalT = finalT;

        this.equX = equX;
        this.equY = equY;
        cache = [];
    }

    public override Graphable ShallowCopy() => new ParametricEquation(InitialT, FinalT, equX, equY);

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        const int step = 10;

        double epsilon = Math.Abs(graph.ScreenSpaceToGraphSpace(new Int2(0, 0)).x
                                - graph.ScreenSpaceToGraphSpace(new Int2(step, 0)).x);

        List<IGraphPart> lines = [];

        Float2 previousPoint = GetFromCache(InitialT, epsilon);
        for (double t = InitialT; t <= FinalT; t += epsilon)
        {
            Float2 currentPoint = GetFromCache(t, epsilon);
            if (graph.IsGraphPointVisible(currentPoint) ||
                graph.IsGraphPointVisible(previousPoint))
                    lines.Add(new GraphLine(previousPoint, currentPoint));
            previousPoint = currentPoint;
        }

        return lines;
    }

    public Graphable Derive() =>
        new ParametricEquation(InitialT, FinalT, GetDerivativeAtPointX, GetDerivativeAtPointY);

    public ParametricDelegate GetXDelegate() => equX;
    public ParametricDelegate GetYDelegate() => equY;

    public double GetDerivativeAtPointX(double t)
    {
        const double step = 1e-3;
        return (equX(t + step) - equX(t)) / step;
    }
    public double GetDerivativeAtPointY(double t)
    {
        const double step = 1e-3;
        return (equY(t + step) - equY(t)) / step;
    }
    public Float2 GetDerivativeAtPoint(double t) =>
        new(GetDerivativeAtPointX(t), GetDerivativeAtPointY(t));

    public Float2 GetPointAt(double t) => GetFromCache(t, 0);

    public override void EraseCache() => cache.Clear();
    protected Float2 GetFromCache(double t, double epsilon)
    {
        (double dist, Float2 nearest, int index) = NearestCachedPoint(t);
        if (dist < epsilon) return new(nearest.x + OffsetX, nearest.y + OffsetY);
        else
        {
            Float2 result = new(equX(t), equY(t));
            cache.Insert(index + 1, (t, result));
            return new(result.x + OffsetX, result.y + OffsetY);
        }
    }
    public override long GetCacheBytes() => cache.Count * 24;

    protected (double dist, Float2 point, int index) NearestCachedPoint(double t)
    {
        if (cache.Count <= 1) return (double.PositiveInfinity, new(double.NaN, double.NaN), -1);
        else if (cache.Count == 1)
        {
            (double resultT, Float2 resultPoint) = cache[0];
            return (Math.Abs(resultT - t), resultPoint, 0);
        }
        else
        {
            int boundA = 0, boundB = cache.Count;
            do
            {
                int boundC = (boundA + boundB) / 2;

                (double thisT, Float2 thisPoint) = cache[boundC];
                if (thisT == t) return (0, thisPoint, boundC);
                else if (thisT > t)
                {
                    boundA = boundC;
                }
                else // thisT < t
                {
                    boundB = boundC;
                }

            } while (boundB - boundA > 1);

            (double resultT, Float2 resultPoint) = cache[boundA];
            return (Math.Abs(resultT - t), resultPoint, boundA);
        }
    }

    public override void Preload(Float2 xRange, Float2 yRange, double step)
    {
        for (double t = InitialT; t <= FinalT; t += step) GetFromCache(t, step);
    }
}

public delegate double ParametricDelegate(double t);
