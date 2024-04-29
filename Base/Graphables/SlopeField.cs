using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Graphing.Graphables;

public class SlopeField : Graphable
{
    private static int slopeFieldNum;

    public double Detail
    {
        get => _detail;
        set
        {
            if (Math.Abs(value - Detail) >= 1e-4)
            {
                // When changing detail, we need to regenerate all
                // the lines. Inefficient, I know. Might be optimized
                // in a future update.
                EraseCache();
            }
            _detail = value;
        }
    }
    private double _detail;

    protected readonly SlopeFieldsDelegate equ;
    protected readonly List<(Float2, GraphLine)> cache;

    public SlopeField(double detail, SlopeFieldsDelegate equ)
    {
        slopeFieldNum++;
        Name = $"Slope Field {slopeFieldNum}";

        this.equ = equ;
        _detail = detail;
        cache = [];
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        double step = 1 / _detail;
        double epsilon = step * 0.5;
        List<IGraphPart> lines = [];

        double minX = Math.Round((graph.MinVisibleGraph.x - 1) / step) * step,
               maxX = Math.Round((graph.MaxVisibleGraph.x + 1) / step) * step,
               minY = Math.Round((graph.MinVisibleGraph.y - 1) / step) * step,
               maxY = Math.Round((graph.MaxVisibleGraph.y + 1) / step) * step;

        for (double x = minX; x < maxX; x += step)
        {
            for (double y = minY; y < maxY; y += step)
            {
                lines.Add(GetFromCache(epsilon, x, y));
            }
        }

        return lines;
    }

    protected GraphLine MakeSlopeLine(Float2 position, double slope)
    {
        double size = _detail;

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

    public override Graphable ShallowCopy() => new SlopeField(_detail, equ);

    public override void EraseCache() => cache.Clear();
    public override long GetCacheBytes() => cache.Count * 48;
    
    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor)
    {
        Float2 nearestPos = new(Math.Round(graphMousePos.x * _detail) / _detail,
                                Math.Round(graphMousePos.y * _detail) / _detail);

        double epsilon = 1 / (_detail * 2.0);
        GraphLine line = GetFromCache(epsilon, nearestPos.x, nearestPos.y);
        double slope = (line.b.y - line.a.y) / (line.b.x - line.a.x);

        if (graphMousePos.x < Math.Min(line.a.x, line.b.x) ||
            graphMousePos.x > Math.Max(line.a.x, line.b.x)) return false;

        double allowedDist = factor * graph.DpiFloat * 10 / 192;

        double lineX = graphMousePos.x,
               lineY = slope * (lineX - nearestPos.x) + nearestPos.y;

        Int2 pointScreen = graph.GraphSpaceToScreenSpace(new Float2(lineX, lineY));
        Int2 mouseScreen = graph.GraphSpaceToScreenSpace(graphMousePos);
        Int2 dist = new(pointScreen.x - mouseScreen.x,
                        pointScreen.y - mouseScreen.y);
        double totalDist = Math.Sqrt(dist.x * dist.x + dist.y * dist.y);
        return totalDist <= allowedDist;
    }
    public override IEnumerable<IGraphPart> GetSelectionItemsToRender(in GraphForm graph, Float2 graphMousePos)
    {
        Float2 nearestPos = new(Math.Round(graphMousePos.x * _detail) / _detail,
                                Math.Round(graphMousePos.y * _detail) / _detail);

        double epsilon = 1 / (_detail * 2.0);
        GraphLine line = GetFromCache(epsilon, nearestPos.x, nearestPos.y);
        double slope = (line.b.y - line.a.y) / (line.b.x - line.a.x);

        double lineX = graphMousePos.x,
               lineY = slope * (lineX - nearestPos.x) + nearestPos.y;
        Float2 point = new(lineX, lineY);

        return
        [
            new GraphUiText($"M = {slope:0.000}", point, ContentAlignment.BottomLeft),
            new GraphUiCircle(point)
        ];
    }

    public override void Preload(Float2 xRange, Float2 yRange, double step)
    {
        for (double x = Math.Ceiling(xRange.x - 1); x < xRange.y + 1; x += 1.0 / _detail)
        {
            for (double y = Math.Ceiling(yRange.x - 1); y < yRange.y + 1; y += 1.0 / _detail)
            {
                GetFromCache(step, x, y);
            }
        }
    }
}

public delegate double SlopeFieldsDelegate(double x, double y);
