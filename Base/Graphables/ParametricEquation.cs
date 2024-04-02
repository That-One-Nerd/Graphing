using Graphing.Abstract;
using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;

namespace Graphing.Graphables;

public class ParametricEquation : Graphable, ITranslatableXY
{
    private static int equationNum;

    public double OffsetX { get; set; }
    public double OffsetY { get; set; }

    public double InitialT { get; set; }
    public double FinalT { get; set; }

    protected readonly ParametricDelegate equX, equY;

    public ParametricEquation(double initialT, double finalT,
                              ParametricDelegate equX, ParametricDelegate equY)
    {
        equationNum++;
        Name = $"Parametric Equation {equationNum}";

        InitialT = initialT;
        FinalT = finalT;

        this.equX = equX;
        this.equY = equY;
    }

    public override Graphable ShallowCopy() => new ParametricEquation(InitialT, FinalT, equX, equY);

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        const int step = 10;

        double epsilon = Math.Abs(graph.ScreenSpaceToGraphSpace(new Int2(0, 0)).x
                                - graph.ScreenSpaceToGraphSpace(new Int2(step / 2, 0)).x) / 5;
        epsilon *= graph.DpiFloat / 192;

        List<IGraphPart> lines = [];

        Float2 previousPoint = GetPointAt(InitialT);
        for (double t = InitialT; t <= FinalT; t += epsilon)
        {
            Float2 currentPoint = GetPointAt(t);
            lines.Add(new GraphLine(previousPoint, currentPoint));
            previousPoint = currentPoint;
        }

        return lines;
    }

    public Float2 GetPointAt(double t)
    {
        return new(equX(t) + OffsetX, equY(t) + OffsetY);
    }
}

public delegate double ParametricDelegate(double t);
