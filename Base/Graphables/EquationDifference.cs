using Graphing.Abstract;
using Graphing.Forms;
using Graphing.Parts;
using System.Collections.Generic;

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
               pB = new(Position, points.y),
               pC = new(Position, (points.x + points.y) / 2);
        return [new GraphUiText($"{points.x - points.y:0.00}", pC),
                new GraphUiCircle(pA), new GraphUiCircle(pB), new GraphLine(pA, pB)];
    }

    public double DistanceAtPoint(double x) => equA.GetValueAt(x) - equB.GetValueAt(x);

    public override Graphable ShallowCopy() => new EquationDifference(Position, equA, equB);

    public Equation ToEquation() => new(DistanceAtPoint)
    {
        Color = Color,
        Name = Name
    };
}
