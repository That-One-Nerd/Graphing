using Graphing.Forms;

namespace Graphing.Parts;

public record struct GraphRectangle : IGraphPart
{
    public Float2 min, max;

    public GraphRectangle()
    {
        min = new();
        max = new();
    }
    
    public static GraphRectangle FromSize(Float2 center, Float2 size) => new()
    {
        min = new(center.x - size.x / 2,
                  center.y - size.y / 2),
        max = new(center.x + size.x / 2,
                  center.y + size.y / 2)
    };
    public static GraphRectangle FromRange(Float2 min, Float2 max) => new()
    {
        min = min,
        max = max
    };

    public void Render(in GraphForm form, in Graphics g, in Pen pen)
    {
        if (!double.IsFinite(max.x) || !double.IsFinite(max.y) ||
            !double.IsFinite(min.x) || !double.IsFinite(min.y)) return;

        if (min.x > max.x) (min.x, max.x) = (max.x, min.x);
        if (min.y > max.y) (min.y, max.y) = (max.y, min.y);

        Int2 start = form.GraphSpaceToScreenSpace(min),
             end = form.GraphSpaceToScreenSpace(max);

        Int2 size = new(end.x - start.x + 1,
                        start.y - end.y);

        if (size.x == 0 || size.y == 0) return;
        g.FillRectangle(pen.Brush, new Rectangle(start.x, end.y, size.x, size.y));
    }
}
