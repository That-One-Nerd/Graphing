using Graphing.Forms;

namespace Graphing.Parts;

public record struct GraphUiCircle : IGraphPart
{
    public Float2 center;
    public int radius;

    public GraphUiCircle()
    {
        center = new();
        radius = 1;
    }
    public GraphUiCircle(Float2 center, int radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public readonly void Render(in GraphForm form, in Graphics g, in Pen pen)
    {
        if (!double.IsFinite(center.x) || !double.IsFinite(center.y) ||
            !double.IsFinite(radius) || radius == 0) return;

        Int2 centerPix = form.GraphSpaceToScreenSpace(center);
        g.FillEllipse(pen.Brush, new Rectangle(new Point(centerPix.x - radius,
                                                         centerPix.y - radius),
                                            new Size(radius * 2, radius * 2)));
    }
}
