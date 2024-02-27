using Graphing.Forms;

namespace Graphing.Graphables;

public class Equation : Graphable
{
    private static int equationNum;

    private readonly EquationDelegate equ;

    public Equation(EquationDelegate equ)
    {
        equationNum++;
        Name = $"Equation {equationNum}";

        this.equ = equ;
    }

    public override IEnumerable<Line2d> GetItemsToRender(in GraphForm graph)
    {
        List<Line2d> lines = [];
        double previousX = graph.MinVisibleGraph.x;
        double previousY = equ(previousX);
        for (int i = 1; i < graph.ClientRectangle.Width; i += 10)
        {
            double currentX = graph.ScreenSpaceToGraphSpace(new Int2(i, 0)).x;
            double currentY = equ(currentX);
            if (Math.Abs(currentY - previousY) <= 10)
            {
                lines.Add(new Line2d(new Float2(previousX, previousY), new Float2(currentX, currentY)));
            }
            previousX = currentX;
            previousY = currentY;
        }
        return lines;
    }

    public EquationDelegate GetDelegate() => equ;
}

public delegate double EquationDelegate(double x);
