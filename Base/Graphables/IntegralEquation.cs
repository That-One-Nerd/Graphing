using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;

namespace Graphing.Graphables;

public class IntegralEquation : Graphable
{
    protected readonly Equation baseEqu;
    protected readonly EquationDelegate baseEquDel;

    public IntegralEquation(Equation baseEquation)
    {
        string oldName = baseEquation.Name, newName;
        if (oldName.StartsWith("Integral of ")) newName = "Second Integral of " + oldName[12..];
        else if (oldName.StartsWith("Second Integral of ")) newName = "Third Integral of " + oldName[19..];
        else newName = "Integral of " + oldName;

        Name = newName;

        baseEqu = baseEquation;
        baseEquDel = baseEquation.GetDelegate();
    }

    public override Graphable DeepCopy() => new IntegralEquation(baseEqu);

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        const int step = 10;
        double epsilon = Math.Abs(graph.ScreenSpaceToGraphSpace(new Int2(0, 0)).x
                                - graph.ScreenSpaceToGraphSpace(new Int2(step / 2, 0)).x) / 5;
        epsilon *= graph.DpiFloat / 192;
        List<IGraphPart> lines = [];

        Int2 originLocation = graph.GraphSpaceToScreenSpace(new Float2(0, 0));

        if (originLocation.x < 0)
        {
            // Origin is off the left side of the screen.
            // Get to the left side from the origin.
            double previousY = 0;
            double start = graph.MinVisibleGraph.x, end = graph.MaxVisibleGraph.x;
            for (double x = 0; x <= start; x += epsilon) previousY += baseEquDel(x) * epsilon;

            // Now we can start.
            double previousX = start;

            for (double x = start; x <= end; x += epsilon)
            {
                double currentX = x, currentY = previousY + baseEquDel(x) * epsilon;
                lines.Add(new GraphLine(new Float2(previousX, previousY), new Float2(currentX, currentY)));

                previousX = currentX;
                previousY = currentY;
            }
        }
        else if (originLocation.x > graph.ClientRectangle.Width)
        {
            // Origin is off the right side of the screen.
            // Get to the right side of the origin.
            double previousY = 0;
            double start = graph.MaxVisibleGraph.x, end = graph.MinVisibleGraph.x;
            for (double x = 0; x >= start; x -= epsilon) previousY -= baseEquDel(x) * epsilon;

            // Now we can start.
            double previousX = start;

            for (double x = start; x >= end; x -= epsilon)
            {
                double currentX = x, currentY = previousY - baseEquDel(x) * epsilon;
                lines.Add(new GraphLine(new Float2(previousX, previousY), new Float2(currentX, currentY)));

                previousX = currentX;
                previousY = currentY;
            }
        }
        else
        {
            // Origin is on-screen.
            // We need to do two cycles.

            // Start with right.
            double start = 0, end = graph.MaxVisibleGraph.x;
            double previousX = start;
            double previousY = 0;

            for (double x = start; x <= end; x += epsilon)
            {
                double currentX = x, currentY = previousY + baseEquDel(x) * epsilon;
                lines.Add(new GraphLine(new Float2(previousX, previousY), new Float2(currentX, currentY)));

                previousX = currentX;
                previousY = currentY;
            }

            // Now do left.
            start = 0;
            end = graph.MinVisibleGraph.x;
            previousX = start;
            previousY = 0;

            for (double x = start; x >= end; x -= epsilon)
            {
                double currentX = x, currentY = previousY - baseEquDel(x) * epsilon;
                lines.Add(new GraphLine(new Float2(previousX, previousY), new Float2(currentX, currentY)));

                previousX = currentX;
                previousY = currentY;
            }
        }

        return lines;
    }

    public Equation AsEquation() => new(GetIntegralAtPoint);

    // Standard integral method.
    // Inefficient for successive calls.
    public double GetIntegralAtPoint(double x)
    {
        EquationDelegate equ = baseEqu.GetDelegate();

        double start = Math.Min(0, x), end = Math.Max(0, x);
        const double step = 1e-3;
        double sum = 0;

        for (double t = start; t <= end; t += step) sum += equ(t) * step;
        if (x < 0) sum = -sum;

        return sum;
    }
}
