using Graphing.Forms;

namespace Graphing.Parts;

public record struct GraphCircle : IGraphPart
{
    public Float2 center;
    public int radius;

    public GraphCircle()
    {
        center = new();
        radius = 1;
    }
    public GraphCircle(Float2 center, int radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public readonly void Render(in GraphForm form, in Graphics g, in Brush brush)
    {
        if (!double.IsNormal(center.x) || !double.IsNormal(center.y) ||
            !double.IsNormal(radius)) return;

        Int2 centerPix = form.GraphSpaceToScreenSpace(center);
        g.FillEllipse(brush, new Rectangle(new Point(centerPix.x - radius,
                                                     centerPix.y - radius),
                                            new Size(radius * 2, radius * 2)));
    }
}
