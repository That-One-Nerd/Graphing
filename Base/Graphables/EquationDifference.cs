using Graphing.Abstract;
using Graphing.Forms;
using Graphing.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Graphing.Graphables;

public class EquationDifference : Graphable, ITranslatableX, IConvertEquation
{
    public bool UngraphWhenConvertedToEquation => true;

    public double Position
    {
        get => _position;
        set
        {
            _position = value;
            points = new Float2(equA.GetValueAt(value), equB.GetValueAt(value));
        }
    }
    private double _position;

    public double OffsetX
    {
        get => Position;
        set => Position = value;
    }

    protected readonly Equation equA, equB;
    protected Float2 points; // X represents equA.y, Y represents equB.y

    public EquationDifference(double position, Equation equA, Equation equB)
    {
        this.equA = equA;
        this.equB = equB;

        Name = $"Difference between {equA.Name} and {equB.Name}";

        Position = position;
    }

    public override IEnumerable<IGraphPart> GetItemsToRender(in GraphForm graph)
    {
        Float2 pA = new(Position, points.x),
               pB = new(Position, points.y);
        return
        [
            new GraphUiCircle(pA),
            new GraphUiCircle(pB),
            new GraphLine(pA, pB)
        ];
    }

    public double DistanceAtPoint(double x) => equA.GetValueAt(x) - equB.GetValueAt(x);

    public override Graphable ShallowCopy() => new EquationDifference(Position, equA, equB);

    public override bool ShouldSelectGraphable(in GraphForm graph, Float2 graphMousePos, double factor)
    {
        Float2 nearestPoint = new(Position, graphMousePos.y);
        double upper = double.Max(points.x, points.y),
               lower = double.Min(points.x, points.y);
        if (nearestPoint.y > upper) nearestPoint.y = upper;
        else if (nearestPoint.y < lower) nearestPoint.y = lower;

        Int2 nearestPixelPoint = graph.GraphSpaceToScreenSpace(nearestPoint);
        Int2 screenMousePos = graph.GraphSpaceToScreenSpace(graphMousePos);

        Int2 diff = new(screenMousePos.x - nearestPixelPoint.x,
                        screenMousePos.y - nearestPixelPoint.y);
        int dist = (int)Math.Sqrt(diff.x * diff.x + diff.y * diff.y);
        return dist < 50 * factor * graph.DpiFloat / 192;
    }
    public override IEnumerable<IGraphPart> GetSelectionItemsToRender(in GraphForm graph, Float2 graphMousePos)
    {
        Float2 nearestPoint = new(Position, graphMousePos.y);
        double upper = double.Max(points.x, points.y),
               lower = double.Min(points.x, points.y);
        if (nearestPoint.y > upper) nearestPoint.y = upper;
        else if (nearestPoint.y < lower) nearestPoint.y = lower;

        return
        [
            new GraphUiText($"Δ = {points.x - points.y:0.000}", nearestPoint, ContentAlignment.MiddleLeft, offsetPix: new Int2(15, 0)),
            new GraphUiCircle(nearestPoint)
        ];
    }

    public Equation ToEquation() => new(DistanceAtPoint)
    {
        Color = Color,
        Name = Name
    };
}
