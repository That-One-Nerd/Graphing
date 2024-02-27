using Graphing.Forms;

namespace Graphing.Graphables;

public class SlopeField : Graphable
{
    private static int slopeFieldNum;

    private readonly SlopeFieldsDelegate equ;
    private readonly double detail;

    public SlopeField(int detail, SlopeFieldsDelegate equ)
    {
        slopeFieldNum++;
        Name = $"Slope Field {slopeFieldNum}";

        this.equ = equ;
        this.detail = detail;
    }

    public override IEnumerable<Line2d> GetItemsToRender(in GraphForm graph)
    {
        List<Line2d> lines = [];

        for (double x = Math.Ceiling(graph.MinVisibleGraph.x - 1); x < graph.MaxVisibleGraph.x + 1; x += 1 / detail)
        {
            for (double y = Math.Ceiling(graph.MinVisibleGraph.y - 1); y < graph.MaxVisibleGraph.y + 1; y += 1 / detail)
            {
                double slope = equ(x, y);
                lines.Add(MakeSlopeLine(new Float2(x, y), slope));
            }
        }

        return lines;
    }

    private Line2d MakeSlopeLine(Float2 position, double slope)
    {
        double size = detail;

        double dirX = size, dirY = slope * size;
        double magnitude = Math.Sqrt(dirX * dirX + dirY * dirY);

        dirX /= magnitude * size * 2;
        dirY /= magnitude * size * 2;

        return new(new(position.x + dirX, position.y + dirY), new(position.x - dirX, position.y - dirY));
    }
}

public delegate double SlopeFieldsDelegate(double x, double y);
