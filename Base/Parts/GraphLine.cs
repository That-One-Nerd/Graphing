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

    public readonly void Render(in GraphForm form, in Graphics g, in Pen pen)
    {
        if (!double.IsFinite(a.x) || !double.IsFinite(a.y) ||
            !double.IsFinite(b.x) || !double.IsFinite(b.y)) return;

        Int2 start = form.GraphSpaceToScreenSpace(a),
             end = form.GraphSpaceToScreenSpace(b);
        g.DrawLine(pen, start, end);
    }
}
