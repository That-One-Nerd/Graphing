using Graphing.Forms;

namespace Graphing.Parts;

public record struct GraphLine : IGraphPart
{
    public Float2 a;
    public Float2 b;

    public GraphLine()
    {
        a = new();
        b = new();
    }
    public GraphLine(Float2 a, Float2 b)
    {
        this.a = a;
        this.b = b;
    }

    public readonly void Render(in GraphForm form, in Graphics g, in Brush brush)
    {
        if (!double.IsNormal(a.x) || !double.IsNormal(a.y) ||
            !double.IsNormal(b.x) || !double.IsNormal(b.y)) return;

        Int2 start = form.GraphSpaceToScreenSpace(a),
             end = form.GraphSpaceToScreenSpace(b);

        Pen pen = new(brush, 3);
        g.DrawLine(pen, start, end);
    }
}
