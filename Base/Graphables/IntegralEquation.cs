using Graphing.Abstract;
using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Graphing.Graphables;

public class IntegralEquation : Graphable, IIntegrable, IDerivable
{
    protected readonly Equation? baseEqu;
    protected readonly EquationDelegate? baseEquDel;

    protected readonly IntegralEquation? altBaseEqu;

    protected readonly bool usingAlt;

    public IntegralEquation(Equation baseEquation)
    {
        string oldName = baseEquation.Name, newName;
        if (oldName.StartsWith("Integral of ")) newName = "Second Integral of " + oldName[12..];
        else if (oldName.StartsWith("Second Integral of ")) newName = "Third Integral of " + oldName[19..];
        else newName = "Integral of " + oldName;

        Name = newName;

        baseEqu = baseEquation;
        baseEquDel = baseEquation.GetDelegate();

        altBaseEqu = null;
        usingAlt = false;
    }
    public IntegralEquation(IntegralEquation baseEquation)
    {
        string oldName = baseEquation.Name, newName;
        if (oldName.StartsWith("Integral of ")) newName = "Second Integral of " + oldName[12..];
        else if (oldName.StartsWith("Second Integral of ")) newName = "Third Integral of " + oldName[19..];
        else newName = "Integral of " + oldName;

        Name = newName;

        baseEqu = null;
        baseEquDel = null;

        altBaseEqu = baseEquation;
        usingAlt = true;
    }

    public override Graphable DeepCopy() => new IntegralEquation(this);

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
            double start = graph.MinVisibleGraph.x, end = graph.MaxVisibleGraph.x;
            SetInternalStepper(start, epsilon);

            // Now we can start.
            double previousX = stepX;
            double previousY = stepY;
            for (double x = start; x <= end; x += epsilon)
            {
                MoveInternalStepper(epsilon);
                lines.Add(new GraphLine(new Float2(previousX, previousY),
                                        new Float2(stepX, stepY)));
                previousX = stepX;
                previousY = stepY;
            }
        }
        else if (originLocation.x > graph.ClientRectangle.Width)
        {
            // Origin is off the right side of the screen.
            // Get to the right side of the origin.
            double start = graph.MaxVisibleGraph.x, end = graph.MinVisibleGraph.x;
            SetInternalStepper(start, epsilon);

            // Now we can start.
            double previousX = stepX;
            double previousY = stepY;
            for (double x = start; x >= end; x -= epsilon)
            {
                MoveInternalStepper(-epsilon);
                lines.Add(new GraphLine(new Float2(previousX, previousY),
                                        new Float2(stepX, stepY)));
                previousX = stepX;
                previousY = stepY;
            }
        }
        else
        {
            // Origin is on-screen.
            // We need to do two cycles.

            // Start with right.
            double start = 0, end = graph.MaxVisibleGraph.x;
            SetInternalStepper(start, epsilon);

            double previousX = stepX;
            double previousY = stepY;
            for (double x = start; x <= end; x += epsilon)
            {
                MoveInternalStepper(epsilon);
                lines.Add(new GraphLine(new Float2(previousX, previousY),
                                        new Float2(stepX, stepY)));
                previousX = stepX;
                previousY = stepY;
            }

            // Now do left.
            start = 0;
            end = graph.MinVisibleGraph.x;
            SetInternalStepper(start, epsilon);

            previousX = stepX;
            previousY = stepY;

            for (double x = start; x >= end; x -= epsilon)
            {
                MoveInternalStepper(-epsilon);
                lines.Add(new GraphLine(new Float2(previousX, previousY),
                                        new Float2(stepX, stepY)));
                previousX = stepX;
                previousY = stepY;
            }
        }

        return lines;
    }

    private double stepX = 0;
    private double stepY = 0;
    private void SetInternalStepper(double x, double dX)
    {
        stepX = 0;
        stepY = 0;
        if (usingAlt) altBaseEqu!.SetInternalStepper(0, dX);

        if (x > 0)
        {
            while (stepX < x) MoveInternalStepper(dX);
        }
        else if (x < 0)
        {
            while (x < stepX) MoveInternalStepper(-dX);
        }
    }
    private void MoveInternalStepper(double dX)
    {
        stepX += dX;
        if (usingAlt)
        {
            altBaseEqu!.MoveInternalStepper(dX);
            stepY += altBaseEqu!.stepY * dX;
        }
        else
        {
            stepY += baseEquDel!(stepX) * dX;
        }
    }

    // Try to avoid using this, as it converts the integral into a
    // far less efficient format (uses the `IntegralAtPoint` method).
    public Equation AsEquation() => new(IntegralAtPoint)
    {
        Name = Name,
        Color = Color
    };

    public Graphable Derive()
    {
        if (usingAlt) return altBaseEqu!.DeepCopy();
        else return (Equation)baseEqu!.DeepCopy();
    }
    public Graphable Integrate() => new IntegralEquation(this);

    // Standard integral method.
    // Inefficient for successive calls.
    public double IntegralAtPoint(double x)
    {
        if (x > 0)
        {
            double start = Math.Min(0, x), end = Math.Max(0, x);
            const double step = 1e-3;
            double sum = 0;

            SetInternalStepper(start, step);
            for (double t = start; t <= end; t += step)
            {
                MoveInternalStepper(step);
                sum += stepY * step;
            }

            return sum;
        }
        else if (x < 0)
        {
            double start = Math.Max(0, x), end = Math.Min(0, x);
            const double step = 1e-3;
            double sum = 0;

            SetInternalStepper(start, step);
            for (double t = start; t >= end; t -= step)
            {
                MoveInternalStepper(-step);
                sum -= stepY * step;
            }

            return sum;
        }
        else return 0;
    }

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor)
    {
        Int2 screenMousePos = graph.GraphSpaceToScreenSpace(graphMousePos);

        Int2 screenPos = graph.GraphSpaceToScreenSpace(new Float2(graphMousePos.x,
                                                                  IntegralAtPoint(graphMousePos.x)));

        double allowedDist = factor * graph.DpiFloat * 80 / 192;

        Int2 dist = new(screenPos.x - screenMousePos.x,
                        screenPos.y - screenMousePos.y);
        double totalDist = Math.Sqrt(dist.x * dist.x + dist.y * dist.y);
        return totalDist <= allowedDist;
    }
    public override Float2 GetSelectedPoint(in GraphForm graph, Float2 graphMousePos) =>
        new(graphMousePos.x, IntegralAtPoint(graphMousePos.x));
}
